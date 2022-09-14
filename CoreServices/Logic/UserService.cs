using Entities.AuthenticationModels;
using Entities.CoreServicesModels.UserModels;

namespace CoreServices.Logic
{
    public class UserService
    {
        private readonly RepositoryManager _repository;

        public UserService(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region User Services

        public IQueryable<UserModel> GetUsers(UserParameters parameters,
            bool trackChanges)
        {
            return _repository.User
                       .FindAll(parameters, trackChanges)
                       .Select(a => new UserModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Name = a.Name,
                           UserName = a.UserName,
                           EmailAddress = a.EmailAddress,
                           PhoneNumber = a.PhoneNumber,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<UserModel>> GetUsersPaged(
                 UserParameters parameters,
                 bool trackChanges)
        {
            return await PagedList<UserModel>.ToPagedList(GetUsers(parameters, trackChanges), parameters.PageNumber, parameters.PageSize);
        }

        public async Task CreateUser(User user)
        {
            if (await FindByUserName(user.UserName, trackChanges: false) != null)
            {
                throw new Exception("User name already registered");
            }

            user.Password = ChangePassword(user.Password);
            _repository.User.Create(user);
        }

        public async Task ChangePassword(int id, ChangePasswordDto model)
        {
            User user = await FindById(id, trackChanges: true);

            if (!CheckUserPassword(user, model.OldPassword))
            {
                throw new Exception("Old password is wrong!");
            }

            user.Password = ChangePassword(model.NewPassword);
        }

        public string ChangePassword(string newPassword)
        {
            return BC.HashPassword(newPassword);
        }

        public async Task<User> FindByRefreshToken(string token, bool trackChanges)
        {
            return await _repository.User.FindByRefreshToken(token, trackChanges);
        }

        public async Task<User> FindByVerificationCode(string code, bool trackChanges)
        {
            return await _repository.User.FindByVerificationCode(code, trackChanges);
        }

        public async Task<User> FindByUserName(string userName, bool trackChanges)
        {
            return await _repository.User.FindByUserName(userName, trackChanges);
        }

        public async Task<User> FindByEmailAddress(string emailAddress, bool trackChanges)
        {
            return await _repository.User.FindByEmailAddress(emailAddress, trackChanges);
        }

        public async Task<User> FindByAdminId(int fk_admin, bool trackChanges)
        {
            return await _repository.User.FindByAdminId(fk_admin, trackChanges);
        }

        public async Task<User> FindById(int id, bool trackChanges)
        {
            return await _repository.User.FindById(id, trackChanges);
        }

        public UserModel GetUserbyId(int id, bool trackChanges)
        {
            return GetUsers(new UserParameters { Id = id }, trackChanges).SingleOrDefault();
        }

        public bool CheckUserPassword(User user, string password)
        {
            return !string.IsNullOrWhiteSpace(password) &&
                    !string.IsNullOrWhiteSpace(user.Password) &&
                    BC.Verify(password, user.Password);
        }

        public async Task DeleteUser(int id)
        {
            User user = await FindById(id, trackChanges: false);
            _repository.User.Delete(user);
        }

        public int GetUsersCount()
        {
            return _repository.User.Count();
        }


        #endregion

        #region Device Services

        public IQueryable<UserDeviceModel> GetDevices(UserDeviceParameters parameters,
         bool trackChanges)
        {
            return _repository.Device
                       .FindAll(parameters, trackChanges)
                       .Select(a => new UserDeviceModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           NotificationToken = a.NotificationToken,
                           DeviceType = a.DeviceType,
                           AppVersion = a.AppVersion,
                           DeviceVersion = a.DeviceVersion,
                           DeviceModel = a.DeviceModel,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<UserDeviceModel>> GetDevicesPaged(
                UserDeviceParameters parameters,
                bool trackChanges)
        {
            return await PagedList<UserDeviceModel>.ToPagedList(GetDevices(parameters, trackChanges), parameters.PageNumber, parameters.PageSize);
        }

        public void CreateDevice(Device device)
        {
            Device oldDevice = _repository.Device.FindByNotificationToken(device.NotificationToken, trackChanges: true);

            if (oldDevice != null)
            {
                oldDevice.Fk_User = device.Fk_User;
                oldDevice.DeviceType = device.DeviceType;
                oldDevice.AppVersion = device.AppVersion;
                oldDevice.DeviceVersion = device.DeviceVersion;
                oldDevice.DeviceModel = device.DeviceModel;
            }
            else
            {
                _repository.Device.Create(device);
            }
        }

        public void DeleteDevice(Device device)
        {
            _repository.Device.Delete(device);
        }

        public async Task<IEnumerable<Device>> FindDevicesByUserId(int id, bool trackChanges)
        {
            return await _repository.Device.FindDevicesByUserId(id, trackChanges);
        }

        public int GetDevicesCount()
        {
            return _repository.Device.Count();
        }


        #endregion

        #region Refresh Token Services
        public IQueryable<RefreshTokenModel> GetRefreshTokens(RefreshTokenParameters parameters,
       bool trackChanges)
        {
            return _repository.RefreshToken
                       .FindAll(parameters, trackChanges)
                       .Select(a => new RefreshTokenModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           Token = a.Token,
                           Expires = a.Expires,
                           CreatedByIp = a.CreatedByIp,
                           Revoked = a.Revoked,
                           RevokedByIp = a.RevokedByIp,
                           ReplacedByToken = a.ReplacedByToken,
                           ReasonRevoked = a.ReasonRevoked,
                           IsExpired = a.IsExpired,
                           IsRevoked = a.IsRevoked,
                           IsActive = a.IsActive


                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<RefreshTokenModel>> GetRefreshTokensPaged(
                RefreshTokenParameters parameters,
                bool trackChanges)
        {
            return await PagedList<RefreshTokenModel>.ToPagedList(GetRefreshTokens(parameters, trackChanges), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<RefreshToken> FindValidRefreshToken(string userName, int refreshTokenTTL)
        {
            int fk_user = FindByUserName(userName, trackChanges: false).Result.Id;

            return await _repository.RefreshToken.FindAll(new RefreshTokenParameters { Fk_User = fk_user, refreshTokenTTL = refreshTokenTTL }, trackChanges: false).OrderBy(a => a.CreatedAt).LastOrDefaultAsync();


        }
        public int GetRefreshTokensCount()
        {
            return _repository.RefreshToken.Count();
        }


        #endregion

        #region Verification Services
        public IQueryable<VerificationModel> GetVerifications(VerificationParameters parameters,
       bool trackChanges)
        {
            return _repository.Verification
                       .FindAll(parameters, trackChanges)
                       .Select(a => new VerificationModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           Code = a.Code,
                           Expires = a.Expires,
                           CreatedByIp = a.CreatedByIp,
                           IsVerified = a.IsVerified,
                           IsExpired = a.IsExpired,
                           IsActive = a.IsActive
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<VerificationModel>> GetVerificationsPaged(
                VerificationParameters parameters,
                bool trackChanges)
        {
            return await PagedList<VerificationModel>.ToPagedList(GetVerifications(parameters, trackChanges), parameters.PageNumber, parameters.PageSize);
        }

        public int GetVerificationsCount()
        {
            return _repository.Verification.Count();
        }

        public async Task<User> Verificate(VerificationDto model, int verificationTTL)
        {
            User user = await FindByVerificationCode(model.Code, trackChanges: true);

            if (user == null)
            {
                throw new Exception("Invalid code");
            }

            Verification verification = user.Verifications.Single(x => x.Code == model.Code);

            if (!verification.IsActive)
            {
                throw new Exception("Invalid code");
            }

            // remove old refresh tokens from account
            RemoveOldVerificationCodes(user, verificationTTL);

            return user;
        }

        public Verification GenerateVerification(string ipAddress, int verificationTTL)
        {
            string code;
            do
            {
                code = RandomGenerator.GenerateInteger(length: 4, minVal: 111111, maxVal: 999999).ToString();

            } while (CheckVerificationCodeExisting(code));

            return new()
            {
                Code = code,
                Expires = DateTime.UtcNow.AddMinutes(verificationTTL),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        private bool CheckVerificationCodeExisting(string code)
        {
            return _repository.Verification.CheckVerificationCodeExisting(code);
        }

        private void RemoveOldVerificationCodes(User user, int verificationTTL)
        {
            // remove old inactive verification codes from account based on TTL in app settings
            _ = user.Verifications.RemoveAll(x =>
                 x.CreatedAt.AddHours(verificationTTL) <= DateTime.UtcNow);
        }

        #endregion

    }
}
