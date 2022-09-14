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
                           parameters.Fk_Team);
        }

        public async Task<Standings> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }
    }

    public static class StandingsRepositoryExtension
    {
        public static IQueryable<Standings> Filter(
            this IQueryable<Standings> Standingss,
            int id,
            int Fk_Season,
            int Fk_Team
            )
        {
            return Standingss.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_Season == 0 || a.Fk_Season == Fk_Season) &&
                                                   (Fk_Team == 0 || a.Fk_Team == Fk_Team));

        }

    }
}
