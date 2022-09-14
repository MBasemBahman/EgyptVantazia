using Entities.CoreServicesModels.AccountModels;
using Entities.DBModels.AccountModels;

namespace Repository.DBModels.AccountModels
{
    public class AccountRepository : RepositoryBase<Account>
    {
        public AccountRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Account> FindAll(AccountParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Accounts,
                           parameters.Fk_Account_Ignored,
                           parameters.Fk_User,
                           parameters.UserName,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo,
                           parameters.LastActiveFrom,
                           parameters.LastActiveTo,
                           parameters.IsLoginBefore,
                           parameters.AccountUserName,
                           parameters.Phone,
                           parameters.Email);

        }

        public async Task<Account> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges).SingleOrDefaultAsync();
        }

        public async Task<Account> FindByUserId(int fK_User, bool trackChanges)
        {
            return await FindByCondition(a => true, trackChanges)
                        .FindByUserId(fK_User)
                        .SingleOrDefaultAsync();
        }

        public int GetIdByUserId(int fK_User)
        {
            return FindByCondition(a => true, trackChanges: false)
                  .FindByUserId(fK_User)
                  .Select(a => a.Id)
                  .SingleOrDefault();
        }

        public new void Create(Account entity)
        {
            if (entity.User != null)
            {
                entity.User.Password = BC.HashPassword(entity.User.Password);
            }
            base.Create(entity);
        }
        public new int Count()
        {
            return base.Count();
        }

        public new void Delete(Account entity)
        {
            base.Delete(entity);
        }
    }

    public static class AccountRepositoryExtension
    {
        public static IQueryable<Account> Filter(
            this IQueryable<Account> accounts,
            int id,
            List<int> fk_Accounts,
            int fk_Account_Ignored,
            int fk_User,
            string UserName,
            DateTime? createdAtFrom,
            DateTime? createdAtTo,
            DateTime? lastActiveFrom,
            DateTime? lastActiveTo,
            bool? isLoginBefore,
            string accountUserName,
            string phone,
            string email)
        {
            return accounts.Where(a => (id == 0 || a.Id == id) &&
                                       (fk_Account_Ignored == 0 || a.Id != fk_Account_Ignored) &&
                                       (isLoginBefore == null || (isLoginBefore == true ? a.User.RefreshTokens.Any() : !a.User.RefreshTokens.Any())) &&
                                       (fk_Accounts == null || !fk_Accounts.Any() || fk_Accounts.Contains(a.Id)) &&
                                       (fk_User == 0 || a.Fk_User == fk_User) &&
                                       (string.IsNullOrWhiteSpace(accountUserName) || a.User.UserName.ToLower().Contains(accountUserName)) &&
                                       (string.IsNullOrWhiteSpace(phone) || a.User.PhoneNumber.ToLower().Contains(phone)) &&
                                       (string.IsNullOrWhiteSpace(email) || a.User.EmailAddress.ToLower().Contains(email)) &&
                                       (string.IsNullOrEmpty(UserName) || a.User.UserName.ToLower() == UserName.ToLower()) &&

                                       (lastActiveFrom == null || (a.User.RefreshTokens.Any() && a.User.RefreshTokens.OrderByDescending(b => b.Id).Select(a => a.CreatedAt).First() >= lastActiveFrom)) &&
                                       (lastActiveTo == null || lastActiveTo == lastActiveFrom || (a.User.RefreshTokens.Any() && a.User.RefreshTokens.OrderByDescending(b => b.Id).Select(a => a.CreatedAt).First() <= lastActiveTo)) &&

                                       (createdAtFrom == null || a.CreatedAt >= createdAtFrom) &&
                                       (createdAtTo == null || createdAtTo == createdAtFrom || a.CreatedAt <= createdAtTo));
        }

        public static IQueryable<Account> FindByUserId(this IQueryable<Account> accounts, int fK_User)
        {
            return accounts.Where(a => a.Fk_User == fK_User);
        }
    }
}
