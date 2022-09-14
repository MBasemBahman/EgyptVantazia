using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamPlayerRepository : RepositoryBase<AccountTeamPlayer>
    {
        public AccountTeamPlayerRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountTeamPlayer> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<AccountTeamPlayer> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(AccountTeamPlayer entity)
        {
            base.Create(entity);
        }

        public new void Delete(AccountTeamPlayer entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class AccountTeamPlayerRepositoryExtension
    {
        public static IQueryable<AccountTeamPlayer> Filter(this IQueryable<AccountTeamPlayer> AccountTeamPlayers, int id)
        {
            return AccountTeamPlayers.Where(a => id == 0 || a.Id == id);
        }

    }
}
