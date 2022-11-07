using Entities.CoreServicesModels.AccountModels;
using Entities.DBModels.AccountModels;

namespace Repository.DBModels.AccountModels
{
    public class AccountRefCodeRepository : RepositoryBase<AccountRefCode>
    {
        public AccountRefCodeRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountRefCode> FindAll(AccountRefCodeParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                       parameters.Fk_RefAccount,
                       parameters.RefCode);
        }

        public async Task<AccountRefCode> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }


    }

    public static class AccountRefCodeRepositoryExtension
    {
        public static IQueryable<AccountRefCode> Filter(
            this IQueryable<AccountRefCode> AccountRefCodes,
            int id,
            int fk_RefAccount,
            string refCode)
        {
            return AccountRefCodes.Where(a => (id == 0 || a.Id == id) &&
                                              (fk_RefAccount == 0 || a.Fk_RefAccount == fk_RefAccount) &&
                                              (string.IsNullOrWhiteSpace(refCode) || a.RefAccount.RefCode == refCode));

        }

    }
}
