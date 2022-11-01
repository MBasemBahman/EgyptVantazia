using Newtonsoft.Json;
using System.Text;

namespace Services
{
    public class WebhookServices
    {
        public static string Webhook_id { get; set; } = "78bc0c96-1c97-4656-ba44-5be42d2fad18";

        public static async Task Send(object Model)
        {
            try
            {
                string ParamsContecnt = JsonConvert.SerializeObject(Model);
                HttpContent Content = new StringContent(ParamsContecnt, Encoding.UTF8, "application/json");

                using HttpClient client = new();
                HttpRequestMessage request = new()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://webhook.site/{Webhook_id}"),
                    Content = Content
                };
                HttpResponseMessage result = await client.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    string json = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
