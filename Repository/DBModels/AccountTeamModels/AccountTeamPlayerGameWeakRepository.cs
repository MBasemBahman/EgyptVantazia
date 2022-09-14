using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamPlayerGameWeakRepository : RepositoryBase<AccountTeamPlayerGameWeak>
    {
        public AccountTeamPlayerGameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountTeamPlayerGameWeak> FindAll(AccountTeamPlayerGameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_AccountTeamPlayer,
                           parameters.Fk_TeamPlayerType);
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
        public static IQueryable<AccountTeamPlayerGameWeak> Filter(
            this IQueryable<AccountTeamPlayerGameWeak> AccountTeamPlayerGameWeaks,
            int id,
            int Fk_AccountTeamPlayer,
            int Fk_TeamPlayerType)

        {
            return AccountTeamPlayerGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_AccountTeamPlayer == 0 || a.Fk_AccountTeamPlayer == Fk_AccountTeamPlayer)&&
                                                   (Fk_TeamPlayerType == 0 || a.Fk_TeamPlayerType == Fk_TeamPlayerType));


        }

    }
}
