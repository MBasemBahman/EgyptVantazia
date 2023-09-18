using Entities.CoreServicesModels.UserModels;

namespace Repository.DBModels.UserModels
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<User> FindAll(
            UserParameters parameters,
            bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.PhoneNumber,
                           parameters.EmailAddress,
                           parameters.Id,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo,
                           parameters.Fk_Accounts);


        }

        public async Task<User> FindByUserName(string userName, bool trackChanges)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            userName = userName.SafeTrim().SafeLower();

            return await FindByCondition(a => a.UserName.ToLower().Trim() == userName, trackChanges: trackChanges)
                         .Include(a => a.RefreshTokens.OrderByDescending(a => a.Id).Skip(0).Take(1))
                         .FirstOrDefaultAsync();
        }

        public async Task<User> FindByEmailAddress(string emailAddress, bool trackChanges)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return null;
            }

            emailAddress = emailAddress.SafeTrim().SafeLower();

            return await FindByCondition(a => a.EmailAddress.ToLower().Trim() == emailAddress, trackChanges: trackChanges)
                         .Include(a => a.RefreshTokens)
                         .FirstOrDefaultAsync();
        }

        public async Task<User> FindById(int id, bool trackChanges)
        {
            return id == 0
                ? null
                : await FindByCondition(a => a.Id == id, trackChanges: trackChanges)
                         .FirstOrDefaultAsync();
        }

        public async Task<User> FindByRefreshToken(string token, bool trackChanges)
        {
            return string.IsNullOrWhiteSpace(token)
                ? null
                : await FindByCondition(a => a.RefreshTokens.Any(b => b.Token == token)
                                              , trackChanges: trackChanges)
                         .Include(a => a.RefreshTokens.Where(b => b.Token == token))
                         .FirstOrDefaultAsync();
        }

        public async Task<User> FindByAccountId(int fk_Account, bool trackChanges)
        {

            return await FindByCondition(a => a.Account.Id == fk_Account, trackChanges: trackChanges)
                            .FirstOrDefaultAsync();
        }

        public async Task<User> FindByVerificationCode(string code, bool trackChanges)
        {
            return string.IsNullOrWhiteSpace(code)
                ? null
                : await FindByCondition(a => a.Verifications.Any(b => b.Code == code)
                                              , trackChanges: trackChanges)
                         .Include(a => a.Verifications.Where(b => b.Code == code))
                         .FirstOrDefaultAsync();
        }

        public async Task<User> FindByAdminId(int fk_admin, bool trackChanges)
        {

            return await FindByCondition(a => a.DashboardAdministrator.Id == fk_admin, trackChanges: trackChanges)
                            .FirstOrDefaultAsync();
        }
    }

    public static class UserRepositoryExtensions
    {
        public static IQueryable<User> Filter(
            this IQueryable<User> users,
            string phoneNumber,
            string emailAddress,
            int id,
            DateTime? createdAtFrom,
            DateTime? createdAtTo,
            List<int> fk_Accounts)
        {
            phoneNumber = phoneNumber.SafeTrim().SafeLower();
            emailAddress = emailAddress.SafeTrim().SafeLower();

            return users.Where(a => (id == 0 || a.Id == id) &&

                                    (fk_Accounts == null || !fk_Accounts.Any() || fk_Accounts.Contains(a.Id)) &&

                                    (createdAtFrom == null || a.CreatedAt >= createdAtFrom) &&

                                    (createdAtTo == null || createdAtTo == createdAtFrom || a.CreatedAt <= createdAtTo) &&

                                    (string.IsNullOrWhiteSpace(phoneNumber) ||
                                     (!string.IsNullOrWhiteSpace(a.PhoneNumber) &&
                                      a.PhoneNumber.ToLower().Contains(phoneNumber))) &&

                                    (string.IsNullOrWhiteSpace(emailAddress) ||
                                     (!string.IsNullOrWhiteSpace(a.EmailAddress) &&
                                      a.EmailAddress.ToLower().Contains(emailAddress))));
        }


    }
}
