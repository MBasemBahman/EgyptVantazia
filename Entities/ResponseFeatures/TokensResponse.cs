using System.Globalization;

namespace Entities.ResponseFeatures
{
    public class TokenResponse
    {
        public string RefreshToken { get; set; }

        public string Expires { get; set; }

        public TokenResponse(string token, DateTime expires)
        {
            RefreshToken = token;
            Expires = expires.AddHours(2).ToString("ddd, dd MMM yyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this).Replace(",", @"\002C");
        }
    }
}
