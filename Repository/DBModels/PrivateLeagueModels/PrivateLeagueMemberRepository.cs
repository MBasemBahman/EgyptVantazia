using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.DBModels.PrivateLeagueModels;


namespace Repository.DBModels.PrivateLeagueModels
{
    public class PrivateLeagueMemberRepository : RepositoryBase<PrivateLeagueMember>
    {
        public PrivateLeagueMemberRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PrivateLeagueMember> FindAll(PrivateLeagueMemberParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Account,
                           parameters.Fk_PrivateLeague,
                           parameters.IsAdmin,
                           parameters.Fk_Season,
                           parameters.FromPoints,
                           parameters.ToPoints);
        }

        public async Task<PrivateLeagueMember> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(PrivateLeagueMember entity)
        {
            if (FindByCondition(a => a.Fk_PrivateLeague == entity.Fk_PrivateLeague && a.Fk_Account == entity.Fk_Account, trackChanges: false).Any())
            {
                PrivateLeagueMember oldEntity = FindByCondition(a => a.Fk_PrivateLeague == entity.Fk_PrivateLeague && a.Fk_Account == entity.Fk_Account, trackChanges: true).First();

                oldEntity.Ranking = entity.Ranking;
                oldEntity.Points = entity.Points;
            }
            else
            {
                base.Create(entity);
            }
        }
    }

    public static class PrivateLeagueMemberRepositoryExtension
    {
        public static IQueryable<PrivateLeagueMember> Filter(
            this IQueryable<PrivateLeagueMember> PrivateLeagueMembers,
            int id,
            int Fk_Account,
            int Fk_PrivateLeague,
            bool? IsAdmin,
            int Fk_Season,
            int? fromPoints,
            int? toPoints)
        {
            return PrivateLeagueMembers.Where(a => (id == 0 || a.Id == id) &&
                                                   (fromPoints == null || a.Points >= fromPoints) &&
                                                   (toPoints == null || a.Points <= toPoints) &&
                                                   (Fk_Account == 0 || a.Fk_Account == Fk_Account) &&
                                                   (Fk_Season == 0 || a.Account.AccountTeams.Any(b => b.Fk_Season == Fk_Season)) &&
                                                   (IsAdmin == null || a.IsAdmin == IsAdmin) &&
                                                   (Fk_PrivateLeague == 0 || a.Fk_PrivateLeague == Fk_PrivateLeague));

        }

    }
}
