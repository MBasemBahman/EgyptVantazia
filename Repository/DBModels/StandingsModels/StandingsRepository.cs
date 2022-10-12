using Entities.CoreServicesModels.StandingsModels;
using Entities.DBModels.StandingsModels;


namespace Repository.DBModels.StandingsModels
{
    public class StandingsRepository : RepositoryBase<Standings>
    {
        public StandingsRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Standings> FindAll(StandingsParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Season,
                           parameters.Fk_Team,
                           parameters._365_For);
        }

        public async Task<Standings> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public async Task<Standings> FindBySeasonAndTeam(int fk_Season, int fk_Team, bool trackChanges)
        {
            return await FindByCondition(a => a.Fk_Season == fk_Season && a.Fk_Team == fk_Team, trackChanges)
                        .SingleOrDefaultAsync();
        }
    }

    public static class StandingsRepositoryExtension
    {
        public static IQueryable<Standings> Filter(
            this IQueryable<Standings> Standingss,
            int id,
            int Fk_Season,
            int Fk_Team,
            int _365_For)
        {
            return Standingss.Where(a => (id == 0 || a.Id == id) &&
                                         (Fk_Season == 0 || a.Fk_Season == Fk_Season) &&
                                         (Fk_Team == 0 || a.Fk_Team == Fk_Team) &&
                                         (_365_For == 0 || a.For == _365_For));

        }

    }
}
