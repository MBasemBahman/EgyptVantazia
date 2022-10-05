using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Services
{
    public class ServicesHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public string BaseUri { get; set; }

        public ServicesHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> OnGet(string uri)
        {
            return await Send(HttpRequestMessage(HttpMethod.Get, uri));
        }
        public async Task<string> OnPost(string uri, object model)
        {
            return await Send(HttpRequestMessage(HttpMethod.Post, uri, model));
        }
        public async Task<string> OnPut(string uri, object model)
        {
            return await Send(HttpRequestMessage(HttpMethod.Put, uri, model));
        }
        public async Task<string> OnDelete(string uri)
        {
            return await Send(HttpRequestMessage(HttpMethod.Delete, uri));
        }

        private async Task<string> Send(HttpRequestMessage httpRequestMessage)
        {
            string returnContent = null;

            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                returnContent = await httpResponseMessage.Content.ReadAsStringAsync();
            }

            return returnContent;
        }
        private HttpRequestMessage HttpRequestMessage(HttpMethod method, string uri, object model)
        {
            if (model == null)
            {
                return HttpRequestMessage(method, uri);
            }

            StringContent modelJson = new(JsonSerializer.Serialize(model), Encoding.UTF8, Application.Json);

            return new(method, BaseUri + uri)
            {
                Content = modelJson
            };
        }
        private HttpRequestMessage HttpRequestMessage(HttpMethod method, string uri)
        {
            return new(method, BaseUri + uri) { };
        }
    }
}
