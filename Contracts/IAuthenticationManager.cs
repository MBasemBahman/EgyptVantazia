using Entities.AuthenticationModels;

namespace Contracts
{
    public interface IAuthenticationManager
    {
        Task<UserAuthenticatedDto> Authenticate(UserForAuthenticationDto userForAuth, string ipAddress);
        Task<UserAuthenticatedDto> Authenticate(string token, string ipAddress);
        Task RevokeToken(string token, string ipAddress);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<UserAuthenticatedDto> GetById(int id);
    }
}
