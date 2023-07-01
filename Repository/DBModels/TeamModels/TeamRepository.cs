using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;
using static Contracts.EnumData.DBModelsEnum;

namespace Repository.DBModels.TeamModels
{
    public class TeamRepository : RepositoryBase<Team>
    {
        public TeamRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<Team> FindAll(TeamParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters._365_TeamId,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo,
                           parameters.IsActive,
                           parameters.Fk_Season,
                           parameters._365_CompetitionsId);
        }

        public async Task<Team> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.TeamLang)
                        .FirstOrDefaultAsync();
        }

        public async Task<Team> FindBy365Id(string id, bool trackChanges)
        {
            return await FindByCondition(a => a._365_TeamId == id, trackChanges)
                        .Include(a => a.TeamLang)
                        .FirstOrDefaultAsync();
        }

        public void UpdateActivation(int _365CompetitionsEnum, bool isActive)
        {
            List<Team> teams = FindByCondition(a => a.Season._365_CompetitionsId == _365CompetitionsEnum.ToString(), trackChanges: true).ToList();
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
                oldEntity.Fk_Season = entity.Fk_Season;
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
            bool? isActive,
            int fk_Season,
            int _365_CompetitionsId)
        {
            return Teams.Where(a => (id == 0 || a.Id == id) &&
                                    (isActive == null || a.IsActive == isActive) &&

                                    (fk_Season == 0 || a.Fk_Season == fk_Season) &&
                                    (_365_CompetitionsId == 0 || a.Season._365_CompetitionsId == _365_CompetitionsId.ToString()) &&

                                    (string.IsNullOrWhiteSpace(_365_TeamId) || a._365_TeamId == _365_TeamId) &&
                                    (createdAtFrom == null || a.CreatedAt >= createdAtFrom) &&
                                    (createdAtTo == null || a.CreatedAt <= createdAtTo));
        }

    }
}
