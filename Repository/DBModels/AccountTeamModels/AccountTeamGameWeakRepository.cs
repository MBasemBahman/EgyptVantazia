using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;

namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamGameWeakRepository : RepositoryBase<AccountTeamGameWeak>
    {
        public AccountTeamGameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountTeamGameWeak> FindAll(AccountTeamGameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_AccountTeam,
                           parameters.Fk_GameWeak,
                           parameters.Fk_Season,
                           parameters.Fk_Account,
                           parameters._365_GameWeakId,
                           parameters.GameWeakFrom,
                           parameters.GameWeakTo,
                           parameters.BenchBoost,
                           parameters.FreeHit,
                           parameters.WildCard,
                           parameters.DoubleGameWeak,
                           parameters.Top_11,
                           parameters.TripleCaptain);

        }

        public async Task<AccountTeamGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public double GetAverageGameWeakPoints(int fk_GameWeak)
        {
            return FindByCondition(a => a.Fk_GameWeak == fk_GameWeak, trackChanges: false).Any() ?
                    FindByCondition(a => a.Fk_GameWeak == fk_GameWeak, trackChanges: false)
                   .Select(a => a.TotalPoints)
                   .Average() : 0;
        }

        public new void Create(AccountTeamGameWeak entity)
        {
            if (!FindByCondition(a => a.Fk_AccountTeam == entity.Fk_AccountTeam && a.Fk_GameWeak == entity.Fk_GameWeak, trackChanges: false).Any())
            {
                base.Create(entity);

                AccountTeam accountTeam = DBContext.Set<AccountTeam>().Find(entity.Fk_AccountTeam);

                accountTeam.FreeTransfer = accountTeam.FreeTransfer >= 1 ? 2 : 1;
            }
        }
    }

    public static class AccountTeamGameWeakRepositoryExtension
    {
        public static IQueryable<AccountTeamGameWeak> Filter(
            this IQueryable<AccountTeamGameWeak> AccountTeamGameWeaks,
            int id,
            int Fk_AccountTeam,
            int Fk_GameWeak,
            int Fk_Season,
            int Fk_Account,
            string _365_GameWeakId,
            int GameWeakFrom,
            int GameWeakTo,
            bool? BenchBoost,
            bool? FreeHit,
            bool? WildCard,
            bool? DoubleGameWeak,
            bool? Top_11,
            bool? TrippleCaptain)

        {
            return AccountTeamGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_AccountTeam == 0 || a.Fk_AccountTeam == Fk_AccountTeam) &&
                                                   (GameWeakFrom == 0 || a.GameWeak._365_GameWeakIdValue >= GameWeakFrom) &&
                                                   (GameWeakTo == 0 || a.GameWeak._365_GameWeakIdValue <= GameWeakTo) &&
                                                   (BenchBoost == null || a.BenchBoost == BenchBoost) &&
                                                   (FreeHit == null || a.FreeHit == FreeHit) &&
                                                   (WildCard == null || a.WildCard == WildCard) &&
                                                   (DoubleGameWeak == null || a.DoubleGameWeak == DoubleGameWeak) &&
                                                   (Top_11 == null || a.Top_11 == Top_11) &&
                                                   (TrippleCaptain == null || a.TripleCaptain == TrippleCaptain) &&
                                                   (string.IsNullOrEmpty(_365_GameWeakId) || a.GameWeak._365_GameWeakId == _365_GameWeakId) &&
                                                   (Fk_Season == 0 || a.AccountTeam.Fk_Season == Fk_Season) &&
                                                   (Fk_Account == 0 || a.AccountTeam.Fk_Account == Fk_Account) &&
                                                   (Fk_GameWeak == 0 || a.Fk_GameWeak == Fk_GameWeak));

        }

    }
}
