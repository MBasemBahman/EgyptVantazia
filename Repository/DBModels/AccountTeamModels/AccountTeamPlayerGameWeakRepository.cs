using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamPlayerGameWeakRepository : RepositoryBase<AccountTeamPlayerGameWeak>
    {
        public AccountTeamPlayerGameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountTeamPlayerGameWeak> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<AccountTeamPlayerGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(AccountTeamPlayerGameWeak entity)
        {
            base.Create(entity);
        }

        public new void Delete(AccountTeamPlayerGameWeak entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class AccountTeamPlayerGameWeakRepositoryExtension
    {
        public static IQueryable<AccountTeamPlayerGameWeak> Filter(this IQueryable<AccountTeamPlayerGameWeak> AccountTeamPlayerGameWeaks, int id)
        {
            return AccountTeamPlayerGameWeaks.Where(a => id == 0 || a.Id == id);
        }

    }
}
