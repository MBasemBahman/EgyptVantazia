using Entities.ServicesModels;
using Newtonsoft.Json;
using System.Text;
using static Entities.EnumData.LogicEnumData;

namespace Services
{
    public class PaymobServices
    {
        private readonly PaymobConfiguration _config;
        private readonly string _baseUrl;
        public PaymobServices(PaymobConfiguration config)
        {
            _config = config;
            _baseUrl = config.BaseUrl;
        }
        #region Common Steps

        // Step 1
        public async Task<string> Authorization()
        {
            string token = null;

            Dictionary<string, string> Params = new()
                {
                    { "api_key" ,_config.ApiKey},
                };

            string ParamsContecnt = JsonConvert.SerializeObject(Params);
            HttpContent Content = new StringContent(ParamsContecnt, Encoding.UTF8, "application/json");

            using HttpClient client = new();
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUrl}/auth/tokens"),
                Content = Content
            };
            HttpResponseMessage result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                string json = await result.Content.ReadAsStringAsync();

                AuthorizationResponseParameters Data = JsonConvert.DeserializeObject<AuthorizationResponseParameters>(json);

                token = Data.Token;
            }

            return token;
        }

        // Step 2
        public async Task<int> OrderRegistration(OrderRegistrationRequestParameters parameters)
        {
            int orderId = 0;

            Dictionary<string, object> Params = new()
                {
                    { "auth_token" ,parameters.Auth_token},
                    { "delivery_needed" ,"false"},
                    { "amount_cents" ,parameters.Amount_cents * 100},
                    { "merchant_order_id" ,parameters.Merchant_order_id},
                    { "items" ,Array.Empty<string>()}
                };

            string ParamsContecnt = JsonConvert.SerializeObject(Params);
            HttpContent Content = new StringContent(ParamsContecnt, Encoding.UTF8, "application/json");

            using HttpClient client = new();
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUrl}/ecommerce/orders"),
                Content = Content
            };
            HttpResponseMessage result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                string json = await result.Content.ReadAsStringAsync();

                OrderRegistrationResponseParameters Data = JsonConvert.DeserializeObject<OrderRegistrationResponseParameters>(json);

                orderId = Data.Id;
            }

            return orderId;
        }

        // Step 3
        public async Task<string> PaymentKey(PaymentKeyRequestParameters parameters, PyamentTypeEnum pyamentType)
        {
            string token = null;

            int integration_id = 0;

            if (pyamentType == PyamentTypeEnum.Credit)
            {
                integration_id = _config.CardLive;
            }
            else if (pyamentType == PyamentTypeEnum.Wallet)
            {
                integration_id = _config.WalletLive;
            }
            else if (pyamentType == PyamentTypeEnum.Kiosk)
            {
                integration_id = _config.KioskLive;
            }

            Dictionary<string, object> Params = new()
                {
                    { "auth_token" ,parameters.Auth_token},
                    { "amount_cents" ,parameters.Amount_cents * 100},
                    { "expiration" ,3600},
                    { "order_id" ,parameters.Order_id},
                    { "currency" ,"EGP"},
                    { "integration_id" ,integration_id},
                    { "billing_data" ,parameters.Billing_data},
                    { "lock_order_when_paid" ,"true"}
                };

            string ParamsContecnt = JsonConvert.SerializeObject(Params);
            HttpContent Content = new StringContent(ParamsContecnt, Encoding.UTF8, "application/json");

            using HttpClient client = new();
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUrl}/acceptance/payment_keys"),
                Content = Content
            };
            HttpResponseMessage result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                string json = await result.Content.ReadAsStringAsync();

                PaymentKeyResponseParameters Data = JsonConvert.DeserializeObject<PaymentKeyResponseParameters>(json);

                token = Data.Token;
            }

            return token;
        }

        #endregion

        // Pay By Credit
        public string GetIframeUrl(string payment_token)
        {
            return $"{_baseUrl}/acceptance/iframes/{_config.IframeId}?payment_token={payment_token}";
        }

        public async Task<string> WalletPayRequest(string payment_token, string walletIdentifier)
        {
            string redirect_url = null;

            Dictionary<string, object> Params = new()
                {
                    { "payment_token" ,payment_token},
                    { "source" , new Dictionary<string, object>
                        {
                            { "identifier" ,walletIdentifier},
                            { "subtype" ,"WALLET"},
                        }
                    }
                };

            string ParamsContecnt = JsonConvert.SerializeObject(Params);
            HttpContent Content = new StringContent(ParamsContecnt, Encoding.UTF8, "application/json");

            using HttpClient client = new();
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUrl}/acceptance/payments/pay"),
                Content = Content
            };
            HttpResponseMessage result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                string json = await result.Content.ReadAsStringAsync();

                WalletPayResponse Data = JsonConvert.DeserializeObject<WalletPayResponse>(json);

                redirect_url = Data.Iframe_redirection_url;
            }

            return redirect_url;
        }

        public async Task<string> KioskPayRequest(string payment_token)
        {
            string bill_reference = null;

            Dictionary<string, object> Params = new()
                {
                    { "payment_token" ,payment_token},
                    { "source" , new Dictionary<string, object>
                        {
                            { "identifier" ,"AGGREGATOR"},
                            { "subtype" ,"AGGREGATOR"},
                        }
                    }
                };

            string ParamsContecnt = JsonConvert.SerializeObject(Params);
            HttpContent Content = new StringContent(ParamsContecnt, Encoding.UTF8, "application/json");

            using HttpClient client = new();
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUrl}/acceptance/payments/pay"),
                Content = Content
            };
            HttpResponseMessage result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                string json = await result.Content.ReadAsStringAsync();

                KioskPayResponse Data = JsonConvert.DeserializeObject<KioskPayResponse>(json);

                bill_reference = Data.Data.Bill_reference;
            }

            return bill_reference;
        }

        // Step 4
        public async Task<bool> CheckOrderStatus(int order_id)
        {
            bool returnData = false;

            try
            {
                using HttpClient client = new();
                HttpRequestMessage request = new()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_baseUrl}/ecommerce/orders/{order_id}/delivery_status"),
                };
                HttpResponseMessage result = await client.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    returnData = true;
                }
            }
            catch (Exception)
            {
            }

            return returnData;
        }
        public async Task<PayWithSavedTokenResponseParameters> PayWithSavedToken(PayWithSavedTokenRequestParameters parameters)
        {
            PayWithSavedTokenResponseParameters returnData = new();

            try
            {
                Dictionary<string, object> Params = new()
                {
                    { "payment_token" ,parameters.Payment_token},
                    { "source" ,parameters.Source},
                };

                string ParamsContecnt = JsonConvert.SerializeObject(Params);
                HttpContent Content = new StringContent(ParamsContecnt, Encoding.UTF8, "application/json");

                using HttpClient client = new();
                HttpRequestMessage request = new()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_baseUrl}/acceptance/payments/pay"),
                    Content = Content
                };
                HttpResponseMessage result = await client.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    string json = await result.Content.ReadAsStringAsync();

                    returnData = JsonConvert.DeserializeObject<PayWithSavedTokenResponseParameters>(json);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return returnData;
        }

    }

    #region Authentication 
    public class AuthorizationResponseParameters
    {
        // Summary:
        //     Authentication token, which is valid for one hour from the creation time.
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
    #endregion

    #region Order Registration
    public class OrderRegistrationRequestParameters
    {
        // Summary:
        //     The authentication token obtained from step 1
        [JsonProperty(PropertyName = "auth_token")]
        public string Auth_token { get; set; }

        // Summary:
        //     The price of the order in cents.
        [JsonProperty(PropertyName = "amount_cents")]
        public int Amount_cents { get; set; }

        // Summary:
        //     A unique alpha-numeric value, example: "E6RR3".
        //     Discard it from the request if your don't need it.
        [JsonProperty(PropertyName = "merchant_order_id")]
        public string Merchant_order_id { get; set; }
    }

    public class OrderRegistrationResponseParameters
    {
        // Summary:
        //     This is the ID of your order in Accept's database, so you can use 
        //     this reference to perform any action to this Order.
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
    #endregion

    #region Payment Key Request

    public class BillingData
    {
        #region Required

        [JsonProperty(PropertyName = "first_name")]
        public string First_name { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string Last_name { get; set; }

        [JsonProperty(PropertyName = "phone_number")]
        public string Phone_number { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        #endregion

        [JsonProperty(PropertyName = "apartment")]
        public string Apartment { get; set; } = "NA";

        [JsonProperty(PropertyName = "floor")]
        public string Floor { get; set; } = "NA";

        [JsonProperty(PropertyName = "street")]
        public string Street { get; set; } = "NA";

        [JsonProperty(PropertyName = "building")]
        public string Building { get; set; } = "NA";

        [JsonProperty(PropertyName = "shipping_method")]
        public string Shipping_method { get; set; } = "NA";

        [JsonProperty(PropertyName = "postal_code")]
        public string Postal_code { get; set; } = "NA";

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; } = "NA";

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; } = "NA";

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; } = "NA";
    }

    public class PaymentKeyRequestParameters
    {
        // Summary:
        //     The authentication token obtained from step 1
        [JsonProperty(PropertyName = "auth_token")]
        public string Auth_token { get; set; }

        // Summary:
        //     The price of the order in cents.
        [JsonProperty(PropertyName = "amount_cents")]
        public int Amount_cents { get; set; }

        // Summary:
        //     The id of the order you want to perform this payment for.
        [JsonProperty(PropertyName = "order_id")]
        public int Order_id { get; set; }

        // Summary:
        //     An identifier for the payment channel you want your customer to pay through.
        [JsonProperty(PropertyName = "integration_id")]
        public int Integration_id { get; set; }

        // Summary:
        //     The billing data related to the customer related to this payment.
        [JsonProperty(PropertyName = "billing_data")]
        public BillingData Billing_data { get; set; }
    }

    public class PaymentKeyResponseParameters
    {
        // Summary:
        //     The payment token which you will use with the pay API.
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
    #endregion

    public class WalletPayResponse
    {
        [JsonProperty(PropertyName = "pending")]
        public string Pending { get; set; }

        [JsonProperty(PropertyName = "success")]
        public string Success { get; set; }

        [JsonProperty(PropertyName = "redirect_url")]
        public string Redirect_url { get; set; }

        [JsonProperty(PropertyName = "iframe_redirection_url")]
        public string Iframe_redirection_url { get; set; }
    }

    public class KioskPayResponse
    {
        [JsonProperty(PropertyName = "pending")]
        public string Pending { get; set; }

        [JsonProperty(PropertyName = "success")]
        public string Success { get; set; }

        [JsonProperty(PropertyName = "data")]
        public KioskPayResponseData Data { get; set; }
    }

    public class KioskPayResponseData
    {
        [JsonProperty(PropertyName = "bill_reference")]
        public string Bill_reference { get; set; }
    }

    #region Transaction Processed Callback

    public class PaymentKeyClaims
    {
        // Summary:
        //     The billing data related to the customer related to this payment.
        [JsonProperty(PropertyName = "billing_data")]
        public BillingData Billing_data { get; set; }
    }

    public class SourceData
    {
        [JsonProperty(PropertyName = "pan")]
        public string Pan { get; set; }
    }

    public class Data
    {
        [JsonProperty(PropertyName = "txn_response_code")]
        public string Txn_response_code { get; set; }
    }

    public class Order
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }

    public class Obj
    {
        // Summary:
        //     The ID of this transaction, you can check it from your Accept portal, 
        //     transaction tab.
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "masked_pan")]
        public string Masked_pan { get; set; }

        [JsonProperty(PropertyName = "merchant_id")]
        public int Merchant_id { get; set; }

        [JsonProperty(PropertyName = "card_subtype")]
        public string Card_subtype { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "order_id")]
        public string Order_id { get; set; }

        [JsonProperty(PropertyName = "user_added")]
        public bool User_added { get; set; }

        // Summary:
        //     A boolean-valued key indicating the status of the transaction whether it 
        //     was successful or not, it would be true if your customer has successfully 
        //     performed his payment.
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "pending")]
        public bool Pending { get; set; }

        // Summary:
        //     An integer field indicating the amount that was paid to this transaction, 
        //     it might be different than the original order price, and it is in cents.
        [JsonProperty(PropertyName = "amount_cents")]
        public int Amount_cents { get; set; }

        [JsonProperty(PropertyName = "order")]
        public Order Order { get; set; }

        [JsonProperty(PropertyName = "source_data")]
        public SourceData Source_data { get; set; }

        [JsonProperty(PropertyName = "data")]
        public Data Data { get; set; }

        [JsonProperty(PropertyName = "payment_key_claims")]
        public PaymentKeyClaims Payment_key_claims { get; set; }
    }

    public class TransactionProcessedCallbackParameters
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "obj")]
        public Obj Obj { get; set; }
    }
    #endregion

    #region Transaction Response Callback

    public class TransactionResponseCallbackParameters
    {
        [JsonProperty(PropertyName = "merchant_order_id")]
        public string Merchant_order_id { get; set; }

        [JsonProperty(PropertyName = "has_parent_transaction")]
        public bool Has_parent_transaction { get; set; }

        [JsonProperty(PropertyName = "is_voided")]
        public bool Is_voided { get; set; }

        [JsonProperty(PropertyName = "profile_id")]
        public int Profile_id { get; set; }

        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }

        [JsonProperty(PropertyName = "is_auth")]
        public bool Is_auth { get; set; }

        [JsonProperty(PropertyName = "refunded_amount_cents")]
        public int Refunded_amount_cents { get; set; }

        [JsonProperty(PropertyName = "is_refunded")]
        public bool Is_refunded { get; set; }

        [JsonProperty(PropertyName = "data_message")]
        public string Data_message { get; set; }

        [JsonProperty(PropertyName = "source_data_type")]
        public string Source_data_type { get; set; }

        [JsonProperty(PropertyName = "error_occured")]
        public bool Error_occured { get; set; }

        [JsonProperty(PropertyName = "is_3d_secure")]
        public bool Is_3d_secure { get; set; }

        [JsonProperty(PropertyName = "amount_cents")]
        public int Amount_cents { get; set; }

        [JsonProperty(PropertyName = "integration_id")]
        public int Integration_id { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "acq_response_code")]
        public int Acq_response_code { get; set; }

        [JsonProperty(PropertyName = "is_void")]
        public bool Is_void { get; set; }

        [JsonProperty(PropertyName = "source_data_sub_type")]
        public string Source_data_sub_type { get; set; }

        [JsonProperty(PropertyName = "is_void")]
        public int Captured_amount { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "pending")]
        public bool Pending { get; set; }

        [JsonProperty(PropertyName = "txn_response_code")]
        public string Txn_response_code { get; set; }

        [JsonProperty(PropertyName = "is_standalone_payment")]
        public bool Is_standalone_payment { get; set; }

        [JsonProperty(PropertyName = "owner")]
        public int Owner { get; set; }

        [JsonProperty(PropertyName = "is_refund")]
        public bool Is_refund { get; set; }

        [JsonProperty(PropertyName = "source_data_pan")]
        public int Source_data_pan { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime Created_at { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }

    #endregion

    #region Pay With Saved Token

    public class Source
    {
        [JsonProperty(PropertyName = "identifier")]
        public string Identifier { get; set; }

        [JsonProperty(PropertyName = "subtype")]
        public string Subtype { get; set; } = "TOKEN";
    }

    public class PayWithSavedTokenRequestParameters
    {
        [JsonProperty(PropertyName = "source")]
        public Source Source { get; set; }

        [JsonProperty(PropertyName = "payment_token")]
        public string Payment_token { get; set; }
    }

    public class PayWithSavedTokenResponseParameters
    {
        [JsonProperty(PropertyName = "pending")]
        public bool Pending { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }

        [JsonProperty(PropertyName = "source_data")]
        public SourceData Source_data { get; set; }
    }
    #endregion
}
