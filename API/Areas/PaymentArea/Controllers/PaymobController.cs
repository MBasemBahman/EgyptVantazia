﻿using API.Areas.PaymentArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.SubscriptionModels;
using Entities.DBModels.AccountModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.SubscriptionModels;
using static Contracts.EnumData.DBModelsEnum;
using static Entities.EnumData.LogicEnumData;

namespace API.Areas.PaymentArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Payment")]
    [ApiExplorerSettings(GroupName = "Payment")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PaymobController : ExtendControllerBase
    {
        private readonly PaymobServices _paymobServices;
        public PaymobController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings,
        PaymobServices paymobServices) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        {
            _paymobServices = paymobServices;
        }

        [HttpPost]
        [Route(nameof(RequestPayment))]
        public async Task<string> RequestPayment([FromBody] RequestPaymentDto model)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            var prevSubscription = _unitOfWork.Subscription.GetSubscriptions(new SubscriptionParameters
            {
                Fk_Account = auth.Fk_Account,
                Fk_Season = season.Id,
                Id = model.Fk_Subscription,
            }, otherLang: false).FirstOrDefault();

            if (prevSubscription != null && !prevSubscription.IsValid)
            {
                throw new Exception("You already get this subscription in this season!");
            }

            if (model.Fk_Subscription == (int)SubscriptionEnum.All)
            {
                if (_unitOfWork.Account.GetAccountSubscriptions(new AccountSubscriptionParameters
                {
                    Fk_Account = auth.Fk_Account,
                    Fk_Season = season.Id,
                    NotEqualSubscriptionId = (int)SubscriptionEnum.Add3MillionsBank
                }, otherLang: false).Any())
                {
                    throw new Exception("You can`t buy this subscription because you already have sub one in this season!");
                }
            }
            else if (model.Fk_Subscription != (int)SubscriptionEnum.All)
            {
                if (_unitOfWork.Account.GetAccountSubscriptions(new AccountSubscriptionParameters
                {
                    Fk_Account = auth.Fk_Account,
                    Fk_Season = season.Id,
                    Fk_Subscription = (int)SubscriptionEnum.All
                }, otherLang: false).Any())
                {
                    throw new Exception("You can`t buy this subscription because you already have super one in this season!");
                }
            }

            if (auth.PhoneNumber.IsEmpty())
            {
                throw new Exception("Please add phone number!");
            }

            if (auth.EmailAddress.IsEmpty())
            {
                throw new Exception("Please add email address!");
            }

            if (model.PyamentType == PyamentTypeEnum.Wallet && model.WalletIdentifier.IsEmpty())
            {
                throw new Exception("Please add wallet number!");
            }

            SubscriptionModel subscription = _unitOfWork.Subscription.GetSubscriptionById(model.Fk_Subscription, otherLang: false);

            int amount_cents = subscription.CostAfterDiscount;

            string auth_token = await _paymobServices.Authorization();

            int order_id = await _paymobServices.OrderRegistration(new OrderRegistrationRequestParameters
            {
                Auth_token = auth_token,
                Amount_cents = amount_cents,
                Merchant_order_id = DateTime.Now.ToString("ddMMyyyhhmmss")
            });

            string payment_token = await _paymobServices.PaymentKey(new PaymentKeyRequestParameters
            {
                Auth_token = auth_token,
                Amount_cents = amount_cents,
                Order_id = order_id,
                Billing_data = new BillingData
                {
                    First_name = auth.Name,
                    Last_name = auth.Name,
                    Phone_number = auth.PhoneNumber,
                    Email = auth.EmailAddress
                }
            }, model.PyamentType);

            if (payment_token.IsEmpty())
            {
                throw new Exception("Payments have something wrong. try again later!");
            }

            string returnUrl = null;

            if (model.PyamentType == PyamentTypeEnum.Credit)
            {
                returnUrl = _paymobServices.GetIframeUrl(payment_token);
            }
            else if (model.PyamentType == PyamentTypeEnum.Wallet)
            {
                returnUrl = await _paymobServices.WalletPayRequest(payment_token, model.WalletIdentifier);
            }
            else if (model.PyamentType == PyamentTypeEnum.Kiosk)
            {
                returnUrl = await _paymobServices.KioskPayRequest(payment_token);
            }

            if (returnUrl.IsEmpty())
            {
                throw new Exception("Payments have something wrong. try again later!");
            }
            else
            {
                _unitOfWork.Account.CreateAccountSubscription(new AccountSubscription
                {
                    Fk_Subscription = subscription.Id,
                    Fk_Account = auth.Fk_Account,
                    Fk_Season = season.Id,
                    Order_id = order_id.ToString(),
                    IsActive = false,
                    Cost = amount_cents
                });
                await _unitOfWork.Save();
            }

            return returnUrl;
        }

        [HttpPost]
        [Route(nameof(TransactionProcessedCallbackAsync))]
        [AllowAll]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> TransactionProcessedCallbackAsync([FromBody] TransactionProcessedCallbackParameters parameters)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            WebhookServices.Send(parameters).Wait();

            if (parameters.Type == "TRANSACTION" && parameters.Obj.Success)
            {
                string email = parameters.Obj.Payment_key_claims.Billing_data.Email;
                string phoneNumber = parameters.Obj.Payment_key_claims.Billing_data.Phone_number;

                int account = _unitOfWork.Account.GetAccounts(new AccountParameters
                {
                    Email = email,
                    Phone = phoneNumber
                }, otherLang: false).Select(a => a.Id).FirstOrDefault();

                if (account > 0)
                {
                    _unitOfWork.Account.CreatePayment(new Payment
                    {
                        Amount = parameters.Obj.Amount_cents / 100,
                        TransactionId = parameters.Obj.Order.Id.ToString(),
                        Fk_Account = account
                    });

                    int accountSubscriptionId = _unitOfWork.Account.GetAccountSubscriptions(new AccountSubscriptionParameters
                    {
                        Order_id = parameters.Obj.Order_id
                    }, otherLang: false).Select(a => a.Id).FirstOrDefault();

                    if (accountSubscriptionId > 0)
                    {
                        AccountSubscription accountSubscription = await _unitOfWork.Account.FindAccountSubscriptionById(accountSubscriptionId, trackChanges: true);
                        accountSubscription.IsActive = true;
                        _unitOfWork.Save().Wait();

                        AccountTeamModel currentTeam = _unitOfWork.AccountTeam.GetCurrentTeam(auth.Fk_Account, accountSubscription.Fk_Season);
                        AccountTeam accounTeam = await _unitOfWork.AccountTeam.FindAccountTeambyId(currentTeam.Id, trackChanges: true);

                        if (accountSubscription.Fk_Subscription == (int)SubscriptionEnum.All)
                        {
                            accounTeam.TripleCaptain++;
                            accounTeam.DoubleGameWeak++;
                            accounTeam.BenchBoost++;
                            accounTeam.Top_11++;
                            accounTeam.IsVip = true;
                        }
                        else if (accountSubscription.Fk_Subscription == (int)SubscriptionEnum.TripleCaptain)
                        {
                            accounTeam.TripleCaptain++;
                        }
                        else if (accountSubscription.Fk_Subscription == (int)SubscriptionEnum.DoubleGameWeak)
                        {
                            accounTeam.DoubleGameWeak++;
                        }
                        else if (accountSubscription.Fk_Subscription == (int)SubscriptionEnum.BenchBoost)
                        {
                            accounTeam.BenchBoost++;
                        }
                        else if (accountSubscription.Fk_Subscription == (int)SubscriptionEnum.Top_11)
                        {
                            accounTeam.Top_11++;
                        }
                        else if (accountSubscription.Fk_Subscription == (int)SubscriptionEnum.Add3MillionsBank)
                        {
                            accounTeam.TotalMoney += 3;
                        }

                        _unitOfWork.Save().Wait();
                    }
                }

            }
            return Ok();
        }

        [HttpPost]
        [Route(nameof(TransactionResponseCallback))]
        [AllowAll]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult TransactionResponseCallback([FromBody] TransactionProcessedCallbackParameters parameters)
        {
            WebhookServices.Send(parameters).Wait();

            return Ok();
        }
    }
}
