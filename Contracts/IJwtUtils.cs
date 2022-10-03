using Entities.DBModels.UserModels;
using Entities.ResponseFeatures;

namespace Contracts
{
    public interface IJwtUtils
    {
        public TokenResponse GenerateJwtToken(int id, int? expires = 60);
        public int? ValidateJwtToken(string token);
        public RefreshToken GenerateRefreshToken(string ipAddress);
    }
}
