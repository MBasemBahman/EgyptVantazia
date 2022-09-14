namespace Entities.ServicesModels
{
    public class FirebaseNotificationModel
    {
        public string MessageHeading { get; set; }

        public string MessageContent { get; set; }

        public string ImgUrl { get; set; }

        public List<string> RegistrationTokens { get; set; }

        public string Topic { get; set; }

        public string OpenType { get; set; }

        public string OpenValue { get; set; }
    }
}
