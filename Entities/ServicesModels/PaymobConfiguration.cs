namespace Entities.ServicesModels
{
    public class PaymobConfiguration
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string IframeId { get; set; }

        public int KioskTest { get; set; }
        public int KioskLive { get; set; }

        public int WalletTest { get; set; }
        public int WalletLive { get; set; }

        public int CardTest { get; set; }
        public int CardLive { get; set; }
    }
}
