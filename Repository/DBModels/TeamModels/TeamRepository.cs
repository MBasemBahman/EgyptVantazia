using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;


namespace Repository.DBModels.TeamModels
{
    public class TeamRepository : RepositoryBase<Team>
    {
        public TeamRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Team> FindAll(TeamParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters._365_TeamId,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo,
                           parameters.IsActive);
        }

        public async Task<Team> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.TeamLang)
                        .SingleOrDefaultAsync();
        }

        public async Task<Team> FindBy365Id(string id, bool trackChanges)
        {
            return await FindByCondition(a => a._365_TeamId == id, trackChanges)
                        .Include(a => a.TeamLang)
                        .SingleOrDefaultAsync();
        }

        public void UpdateActivation(bool isActive)
        {
            List<Team> teams = FindByCondition(a => true, trackChanges: true).ToList();
            teams.ForEach(a => a.IsActive = isActive);
        }

        public new void Create(Team entity)
        {
            if (entity._365_TeamId.IsExisting() && FindByCondition(a => a._365_TeamId == entity._365_TeamId, trackChanges: false).Any())
            {
                Team oldEntity = FindByCondition(a => a._365_TeamId == entity._365_TeamId, trackChanges: true)
                                .Include(a => a.TeamLang)
                                .First();

                //oldEntity.Name = entity.Name;
                oldEntity._365_TeamId = entity._365_TeamId;
                //oldEntity.TeamLang.Name = entity.TeamLang.Name;
                oldEntity.IsActive = entity.IsActive;
            }
            else
            {
                entity.TeamLang ??= new TeamLang
                {
                    Name = entity.Name,
                };
                base.Create(entity);
            }
        }
    }

    public static class TeamRepositoryExtension
    {
        public static IQueryable<Team> Filter(
            this IQueryable<Team> Teams,
            int id,
            string _365_TeamId,
            DateTime? createdAtFrom,
            DateTime? createdAtTo,
            bool? isActive)
        {
            return Teams.Where(a => (id == 0 || a.Id == id) &&
                                    (isActive == null || a.IsActive == isActive) &&
                                    (string.IsNullOrWhiteSpace(_365_TeamId) || a._365_TeamId == _365_TeamId) &&
                                    (createdAtFrom == null || a.CreatedAt >= createdAtFrom) &&
                                    (createdAtTo == null || a.CreatedAt <= createdAtTo));
        }

    }
}
