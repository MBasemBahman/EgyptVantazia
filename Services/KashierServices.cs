using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Entities.EnumData.LogicEnumData;

namespace Services
{
    public class KashierServices
    {
        public readonly KashierConfiguration _config;
        public KashierServices(KashierConfiguration config)
        {
            _config = config;
        }

        public string GetIframeUrl(PyamentTypeEnum paymentType, int amount, string orderId, bool inTesting)
        {
            string hash = CreateHash(amount, orderId, inTesting);
            string mode = inTesting ? "test" : "live";
            string allowedMethods = "card";

            if (paymentType == PyamentTypeEnum.Credit)
            {
                allowedMethods = "card";
            }
            else if (paymentType == PyamentTypeEnum.Wallet)
            {
                allowedMethods = "wallet";
            }
            else if (paymentType == PyamentTypeEnum.Kiosk)
            {
                allowedMethods = "fawry";
            }

            return @$"https://checkout.kashier.io/?merchantId={_config.Mid}&orderId={orderId}&amount={amount}&currency=EGP&hash={hash}&mode={mode}&merchantRedirect={_config.MerchantRedirect}&serverWebhook={_config.ServerWebhook}&allowedMethods={allowedMethods}&redirectMethod=get&brandColor=%23df1d40&display=ar";
        }

        public async Task<bool> Refund(string transactionId, string orderId, int amount, string reason)
        {
            string _baseUrl = _config.InTesting ? "https://test-fep.kashier.io" : "https://fep.kashier.io";

            KashierRefundModelParameters Params = new()
            {
                ApiOperation = "REFUND",
                Reason = reason,
                Transaction = new KashierRefundTransaction
                {
                    Amount = amount,
                    TargetTransactionId = transactionId,
                }
            };

            using HttpClient client = new();
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUrl}/v3/orders/{orderId}"),
                Content = new StringContent(JsonConvert.SerializeObject(Params), Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Authorization", _config.InTesting ? _config.SecretTestKey : _config.SecretLiveKey);

            HttpResponseMessage result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                string json = await result.Content.ReadAsStringAsync();

                return true;
            }

            return false;
        }


        #region Hashing

        public string CreateHash(int amount, string orderId, bool inTesting)
        {
            string mid = _config.Mid; //your merchant id
            string currency = "EGP";
            string key = inTesting ? _config.ApiTestKey : _config.ApiLiveKey;
            string message = "/?payment=" + mid + "." + orderId + "." + amount.ToString() + "." + currency;

            return CreateHash(message, key);
        }

        private static string CreateHash(string message, string key)
        {
            ASCIIEncoding encoding = new();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(message);
            HMACSHA256 hmacmd256 = new(keyByte);
            byte[] hashmessage = hmacmd256.ComputeHash(messageBytes);
            return ByteToString(hashmessage).ToLower();
        }

        private static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return sbinary;
        }

        public bool ValidateSignature(KashierData body, string hash)
        {
            string path = "";

            PropertyInfo[] propertyInfos = typeof(KashierData).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (string key in body.SignatureKeys)
            {
                if (key.Contains('.'))
                {
                    string[] nestedProperties = key.Split(".");
                    string parentProperty = nestedProperties[0];
                    string nestedProperty = nestedProperties[1];

                    PropertyInfo parentObjectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(parentProperty, StringComparison.InvariantCultureIgnoreCase));

                    object parentObjectValue = parentObjectProperty.GetValue(body);
                    if (parentObjectValue != null)
                    {
                        PropertyInfo nestedObjectProperty = parentObjectValue.GetType().GetProperty(nestedProperty);

                        if (nestedObjectProperty != null)
                        {
                            string value = nestedObjectProperty.GetValue(parentObjectValue)?.ToString();
                            path = path + "&" + key + "=" + (nestedObjectProperty.PropertyType == typeof(bool) ? value.ToLower() : value);
                        }
                    }
                }
                else
                {
                    PropertyInfo objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));

                    if (objectProperty != null)
                    {
                        string value = objectProperty.GetValue(body)?.ToString();
                        path = path + "&" + key + "=" + (objectProperty.PropertyType == typeof(bool) ? value.ToLower() : value);
                    }
                }
            }

            path = path.Replace(" | ", "%20%7C%20");

            string hashed = CreateHash(path[1..], _config.InTesting ? _config.ApiTestKey : _config.ApiLiveKey);

            return hash == hashed;
        }

        #endregion

        public class KashierRedirectParameters
        {
            public string PaymentStatus { get; set; }
            public string CardDataToken { get; set; }
            public string MaskedCard { get; set; }
            public string MerchantOrderId { get; set; }
            public string OrderId { get; set; }
            public string CardBrand { get; set; }
            public string OrderReference { get; set; }
            public string TransactionId { get; set; }
            public int Amount { get; set; }
            public string Currency { get; set; }
            public string Signature { get; set; }
            public string Mode { get; set; }

            public string TransactionResponseMessage { get; set; }

            public bool InTesting => Mode == "test";
            public bool IsSuccess => PaymentStatus == "SUCCESS";
        }

        #region Refund
        public class KashierRefundModelParameters
        {
            [JsonProperty("apiOperation")]
            public string ApiOperation { get; set; }

            [JsonProperty("reason")]
            public string Reason { get; set; }

            [JsonProperty("transaction")]
            public KashierRefundTransaction Transaction { get; set; }
        }

        public class KashierRefundTransaction
        {
            [JsonProperty("amount")]
            public int Amount { get; set; }

            [JsonProperty("targetTransactionId")]
            public string TargetTransactionId { get; set; }
        }
        #endregion

        #region KashierResponse 

        public class KashierTransactionResponseModel
        {
            [JsonProperty("event")]
            public string Event { get; set; }

            [JsonProperty("data")]
            public KashierData Data { get; set; }

            [JsonProperty("hash")]
            public string Hash { get; set; }
        }

        public class Kashier_3DSecure
        {
            [JsonProperty("processACSRedirectURL")]
            public string ProcessACSRedirectURL { get; set; }
        }

        public class KashierCard
        {
            [JsonProperty("cardInfo")]
            public KashierCardInfo CardInfo { get; set; }

            [JsonProperty("amount")]
            public int Amount { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }
        }

        public class KashierCardInfo
        {
            [JsonProperty("cardHolderName")]
            public string CardHolderName { get; set; }

            [JsonProperty("cardBrand")]
            public string CardBrand { get; set; }

            [JsonProperty("maskedCard")]
            public string MaskedCard { get; set; }

            [JsonProperty("cardDataToken")]
            public string CardDataToken { get; set; }

            [JsonProperty("agreement")]
            public object Agreement { get; set; }

            [JsonProperty("ccvToken")]
            public string CcvToken { get; set; }

            [JsonProperty("expiryMonth")]
            public string ExpiryMonth { get; set; }

            [JsonProperty("expiryYear")]
            public string ExpiryYear { get; set; }

            [JsonProperty("save")]
            public object Save { get; set; }
        }

        public class KashierData
        {
            [JsonProperty("merchantOrderId")]
            public string MerchantOrderId { get; set; }

            [JsonProperty("kashierOrderId")]
            public string KashierOrderId { get; set; }

            [JsonProperty("orderReference")]
            public string OrderReference { get; set; }

            [JsonProperty("transactionId")]
            public string TransactionId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            public bool IsSuccess => Status == "SUCCESS";

            [JsonProperty("method")]
            public string Method { get; set; }

            [JsonProperty("creationDate")]
            public DateTime CreationDate { get; set; }

            [JsonProperty("amount")]
            public int Amount { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("card")]
            public KashierCard Card { get; set; }

            [JsonProperty("metaData")]
            public KashierMetaData MetaData { get; set; }

            [JsonProperty("sourceOfFunds")]
            public KashierSourceOfFunds SourceOfFunds { get; set; }

            [JsonProperty("transactionResponseCode")]
            public string TransactionResponseCode { get; set; }

            [JsonProperty("transactionResponseMessage")]
            public TransactionResponseMessage TransactionResponseMessage { get; set; }

            [JsonProperty("channel")]
            public string Channel { get; set; }

            [JsonProperty("merchantDetails")]
            public KashierMerchantDetails MerchantDetails { get; set; }

            [JsonProperty("installmentPlan")]
            public KashierInstallmentPlan InstallmentPlan { get; set; }

            [JsonProperty("signatureKeys")]
            public List<string> SignatureKeys { get; set; }
        }

        public class KashierInstallmentPlan
        {
        }

        public class KashierMerchant
        {
            [JsonProperty("merchantRedirectURL")]
            public string MerchantRedirectURL { get; set; }
        }

        public class KashierMerchantDetails
        {
            [JsonProperty("businessEmail")]
            public string BusinessEmail { get; set; }
        }

        public class KashierMetaData
        {
            [JsonProperty("termsAndConditions")]
            public TermsAndConditions TermsAndConditions { get; set; }
        }

        public class KashierSourceOfFunds
        {
            [JsonProperty("cardInfo")]
            public KashierCardInfo CardInfo { get; set; }

            [JsonProperty("3DSecure")]
            public Kashier_3DSecure _3DSecure { get; set; }
        }

        public class TermsAndConditions
        {
            [JsonProperty("ip")]
            public string Ip { get; set; }
        }

        public class TransactionResponseMessage
        {
            [JsonProperty("en")]
            public string En { get; set; }

            [JsonProperty("ar")]
            public string Ar { get; set; }
        }



        #endregion

        public class KashierConfiguration
        {
            public string ApiTestKey { get; set; }
            public string ApiLiveKey { get; set; }
            public string SecretTestKey { get; set; }
            public string SecretLiveKey { get; set; }
            public string Mid { get; set; }

            public bool InTesting { get; set; }

            public string MerchantRedirect { get; set; }
            public string ServerWebhook { get; set; }
        }
    }
}
