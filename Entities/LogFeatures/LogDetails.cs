namespace Entities.LogFeatures
{
    public class LogDetails
    {
        public string Host { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public string ContentType { get; set; }
        public string QueryString { get; set; }
        public string Body { get; set; }
        public string ErrorMessage { get; set; }
        public string ExceptionMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
