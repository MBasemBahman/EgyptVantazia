using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.TeamModels
{
    public class TeamRepository : RepositoryBase<Team>
    {
        public TeamRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Team> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<Team> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.TeamLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(Team entity)
        {
            entity.TeamLang ??= new TeamLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class TeamRepositoryExtension
    {
        public static IQueryable<Team> Filter(this IQueryable<Team> Teams, int id)
        {
            return Teams.Where(a => id == 0 || a.Id == id);
        }

    }
}
