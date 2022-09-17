using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.DBModels.NewsModels;
using Entities.DBModels.PrivateLeagueModels;
using Entities.RequestFeatures;
using Services;

namespace Repository.DBModels.PrivateLeagueModels
{
    public class PrivateLeagueRepository : RepositoryBase<PrivateLeague>
    {
        public PrivateLeagueRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PrivateLeague> FindAll(PrivateLeagueParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Account,
                           parameters.UniqueCode,
                           parameters.IsAdmin);
        }

        public async Task<PrivateLeague> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(PrivateLeague entity)
        {
            string uniqueCode = RandomGenerator.GenerateString(10);
            while (FindByCondition(a => a.UniqueCode == uniqueCode, trackChanges: false).Any())
            {
                uniqueCode = RandomGenerator.GenerateString(10);
            }

            entity.UniqueCode = uniqueCode;
            base.Create(entity);
        }
    }

    public static class PrivateLeagueRepositoryExtension
    {
        public static IQueryable<PrivateLeague> Filter(
            this IQueryable<PrivateLeague> PrivateLeagues,
            int id,
            int Fk_Account,
            string UniqueCode,
            bool? IsAdmin)
        {
            return PrivateLeagues.Where(a => (id == 0 || a.Id == id) &&
                                             (Fk_Account == 0 || a.PrivateLeagueMembers.Any(b => b.Fk_Account == Fk_Account)) &&
                                             (IsAdmin == null || a.PrivateLeagueMembers.Any(b => b.IsAdmin == IsAdmin)) &&
                                             (string.IsNullOrEmpty(UniqueCode) || a.UniqueCode == UniqueCode));
        }

    }
}
