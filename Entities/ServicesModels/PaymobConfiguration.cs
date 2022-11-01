namespace Entities.ServicesModels
{
    public class PaymobConfiguration
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string IframeId { get; set; }
        public int LiveIntegrationId { get; set; }
        public int TestIntegrationId { get; set; }
    }
}
