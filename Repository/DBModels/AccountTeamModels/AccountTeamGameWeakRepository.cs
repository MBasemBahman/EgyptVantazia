using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamGameWeakRepository : RepositoryBase<AccountTeamGameWeak>
    {
        public AccountTeamGameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountTeamGameWeak> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id
                           );
        }

        public async Task<AccountTeamGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(AccountTeamGameWeak entity)
        {
            base.Create(entity);
        }

        public new void Delete(AccountTeamGameWeak entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class AccountTeamGameWeakRepositoryExtension
    {
        public static IQueryable<AccountTeamGameWeak> Filter(
            this IQueryable<AccountTeamGameWeak> AccountTeamGameWeaks,
            int id
            )

        {
            return AccountTeamGameWeaks.Where(a => (id == 0 || a.Id == id)
                                                  );
        }

    }
}
