using API.Areas.PaymentArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.SubscriptionModels;
using Entities.DBModels.AccountModels;
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

            Entities.CoreServicesModels.SeasonModels.SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

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
