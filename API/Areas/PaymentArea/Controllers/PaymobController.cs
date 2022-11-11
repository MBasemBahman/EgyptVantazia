﻿using API.Controllers;
using Entities.CoreServicesModels.AccountModels;
using Entities.DBModels.AccountModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        [AllowAll]
        public async Task<string> RequestPayment([FromQuery, BindRequired] PyamentTypeEnum pyamentType)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            if (auth.PhoneNumber.IsEmpty())
            {
                throw new Exception("Please add phone number!");
            }

            if (auth.EmailAddress.IsEmpty())
            {
                throw new Exception("Please add email address!");
            }

            int amount_cents = 100; // 100 LE

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
            }, pyamentType);

            if (payment_token.IsEmpty())
            {
                throw new Exception("Payments have something wrong. try again later!");
            }

            string returnUrl = null;

            if (pyamentType == PyamentTypeEnum.Credit)
            {
                returnUrl = _paymobServices.GetIframeUrl(payment_token);
            }
            else if (pyamentType == PyamentTypeEnum.Wallet)
            {
                returnUrl = await _paymobServices.WalletPayRequest(payment_token);
            }
            else if (pyamentType == PyamentTypeEnum.Kiosk)
            {
                returnUrl = await _paymobServices.KioskPayRequest(payment_token);
            }

            return returnUrl;
        }

        [HttpPost]
        [Route(nameof(TransactionProcessedCallback))]
        [AllowAll]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult TransactionProcessedCallback([FromBody] TransactionProcessedCallbackParameters parameters)
        {
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

                int subscription = _unitOfWork.Subscription
                                              .GetSubscriptions(new Entities.CoreServicesModels.SubscriptionModels.SubscriptionParameters(), otherLang: false)
                                              .Select(a => a.Id)
                                              .FirstOrDefault();

                if (account > 0 && subscription > 0)
                {
                    _unitOfWork.Account.CreatePayment(new Payment
                    {
                        Amount = parameters.Obj.Amount_cents / 100,
                        TransactionId = parameters.Obj.Order.Id.ToString(),
                        Fk_Account = account
                    });

                    var season = _unitOfWork.Season.GetCurrentSeason();

                    _unitOfWork.Account.CreateAccountSubscription(new AccountSubscription
                    {
                        Fk_Account = account,
                        Fk_Subscription = subscription,
                        Fk_Season = season.Id
                    });

                    _unitOfWork.Save().Wait();
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
