using Entities.CoreServicesModels.StandingsModels;
using Entities.DBModels.StandingsModels;


namespace Repository.DBModels.StandingsModels
{
    public class StandingsRepository : RepositoryBase<Standings>
    {
        public StandingsRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<Standings> FindAll(StandingsParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Season,
                           parameters.Fk_Team,
                           parameters._365_For,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo,
                           parameters.DashboardSearch);
        }

        public async Task<Standings> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public async Task<Standings> FindBySeasonAndTeam(int fk_Season, int fk_Team, bool trackChanges)
        {
            return await FindByCondition(a => a.Fk_Season == fk_Season && a.Fk_Team == fk_Team, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(Standings entity)
        {
            if (FindByCondition(a => a.Fk_Season == entity.Fk_Season && a.Fk_Team == entity.Fk_Team, trackChanges: false).Any())
            {
                Standings oldEntity = FindByCondition(a => a.Fk_Season == entity.Fk_Season && a.Fk_Team == entity.Fk_Team, trackChanges: true)
                                .First();

                oldEntity.Fk_Season = entity.Fk_Season;
                oldEntity.Fk_Team = entity.Fk_Team;
                oldEntity.GamePlayed = entity.GamePlayed;
                oldEntity.GamesWon = entity.GamesWon;
                oldEntity.GamesLost = entity.GamesLost;
                oldEntity.GamesEven = entity.GamesEven;
                oldEntity.Against = entity.Against;
                oldEntity.Ratio = entity.Ratio;
                oldEntity.Strike = entity.Strike;
                oldEntity.Position = entity.Position;
            }
            else
            {
                base.Create(entity);
            }
        }
    }

    public static class StandingsRepositoryExtension
    {
        public static IQueryable<Standings> Filter(
            this IQueryable<Standings> Standingss,
            int id,
            int Fk_Season,
            int Fk_Team,
            int _365_For,
            DateTime? createdAtFrom,
            DateTime? createdAtTo,
            string dashboardSearch)
        {
            return Standingss.Where(a => (id == 0 || a.Id == id) &&
                                         
                                         (string.IsNullOrEmpty(dashboardSearch) || 
                                          a.Id.ToString().Contains(dashboardSearch) ||
                                          a.Team.Name.Contains(dashboardSearch) ||
                                          a.Season.Name.Contains(dashboardSearch) ||
                                          a.GamesWon.ToString().Contains(dashboardSearch)) &&

                                         (Fk_Season == 0 || a.Fk_Season == Fk_Season) &&
                                         (Fk_Team == 0 || a.Fk_Team == Fk_Team) &&
                                         (_365_For == 0 || a.For == _365_For) &&
                                         (createdAtFrom == null || a.CreatedAt >= createdAtFrom) &&
                                         (createdAtTo == null || a.CreatedAt <= createdAtTo));

        }

    }
}
