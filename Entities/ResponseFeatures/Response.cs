using System.Text;

namespace Entities.ResponseFeatures
{
    public class Response
    {
        public Response()
        {
        }

        public Response(bool Success)
        {
            this.Success = Success;
        }

        public bool Success { get; private set; }

        public string ErrorMessage { get; set; } = "";

        public string ExceptionMessage { get; set; } = "";

        public override string ToString()
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(ErrorMessage);
            ErrorMessage = Convert.ToBase64String(plainTextBytes);

            return JsonConvert.SerializeObject(this).Replace(",", @"\002C");
        }
    }
}
