using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamRepository : RepositoryBase<AccountTeam>
    {
        public AccountTeamRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountTeam> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<AccountTeam> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(AccountTeam entity)
        {
            base.Create(entity);
        }

        public new void Delete(AccountTeam entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class AccountTeamRepositoryExtension
    {
        public static IQueryable<AccountTeam> Filter(
            this IQueryable<AccountTeam> AccountTeams,
            int id
            )
        { 
            return AccountTeams.Where(a => id == 0 || a.Id == id);
        }

    }
}
