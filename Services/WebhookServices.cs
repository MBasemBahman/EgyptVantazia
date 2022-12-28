using Newtonsoft.Json;
using System.Text;

namespace Services
{
    public class WebhookServices
    {
        public static string Webhook_id { get; set; } = "3e912c04-952c-44b5-abb4-3d7890fe3a7c";

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
