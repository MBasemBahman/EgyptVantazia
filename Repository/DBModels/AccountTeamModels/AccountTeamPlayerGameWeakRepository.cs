using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;

namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamPlayerGameWeakRepository : RepositoryBase<AccountTeamPlayerGameWeak>
    {
        public AccountTeamPlayerGameWeakRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<AccountTeamPlayerGameWeak> FindAll(AccountTeamPlayerGameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_AccountTeamPlayer,
                           parameters.Fk_TeamPlayerType,
                           parameters.Fk_Player,
                           parameters.Fk_AccountTeam,
                           parameters.Fk_Account,
                           parameters.Fk_Season,
                           parameters.Fk_GameWeak,
                           parameters.IsPrimary,
                           parameters.GameWeakFrom,
                           parameters.GameWeakTo,
                           parameters.IsTransfer);
        }

        public async Task<AccountTeamPlayerGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public void ResetPoints(int fk_AccountTeam, int fk_GameWeak)
        {
            List<AccountTeamPlayerGameWeak> players = FindAll(new AccountTeamPlayerGameWeakParameters
            {
                Fk_AccountTeam = fk_AccountTeam,
                Fk_GameWeak = fk_GameWeak
            }, trackChanges: true).ToList();

            players.ForEach(a => a.Points = null);
        }

        public void ResetTeamPlayers(int fk_AccountTeam, int fk_GameWeak, int fk_AccountTeamGameWeak)
        {
            AccountTeamGameWeak accountTeamGameWeak = DBContext.AccountTeamGameWeaks
                                                               .Where(a => a.Id == fk_AccountTeamGameWeak && a.TansfarePoints < 0)
                                                               .FirstOrDefault();
            if (accountTeamGameWeak != null)
            {
                accountTeamGameWeak.TansfarePoints = 0;
            }

            List<AccountTeamPlayerGameWeak> players = FindByCondition(a => a.Fk_GameWeak == fk_GameWeak && a.AccountTeamPlayer.Fk_AccountTeam == fk_AccountTeam, trackChanges: true).ToList();

            players.ForEach(player => Delete(player));
        }

        public new void Create(AccountTeamPlayerGameWeak entity)
        {
            if (FindByCondition(a => a.Fk_AccountTeamPlayer == entity.Fk_AccountTeamPlayer && a.Fk_GameWeak == entity.Fk_GameWeak, false).Any())
            {
                AccountTeamPlayerGameWeak oldEntity = FindByCondition(a => a.Fk_AccountTeamPlayer == entity.Fk_AccountTeamPlayer && a.Fk_GameWeak == entity.Fk_GameWeak, trackChanges: true).First();

                oldEntity.Fk_TeamPlayerType = entity.Fk_TeamPlayerType;
                oldEntity.IsPrimary = entity.IsPrimary;
                oldEntity.Order = entity.Order;
                oldEntity.Points = entity.Points;
                oldEntity.HavePoints = entity.HavePoints;
                oldEntity.HavePointsInTotal = entity.HavePointsInTotal;
            }
            else
            {
                base.Create(entity);
            }
        }

        public void ResetAccountTeamPlayer(int fk_AccountTeam, int fk_GameWeak)
        {
            List<AccountTeamPlayerGameWeak> players = FindAll(new AccountTeamPlayerGameWeakParameters
            {
                Fk_AccountTeam = fk_AccountTeam,
                Fk_GameWeak = fk_GameWeak
            }, trackChanges: true).ToList();

            players.ForEach(a => Delete(a));
        }
    }

    public static class AccountTeamPlayerGameWeakRepositoryExtension
    {
        public static IQueryable<AccountTeamPlayerGameWeak> Filter(
            this IQueryable<AccountTeamPlayerGameWeak> AccountTeamPlayerGameWeaks,
            int id,
            int fk_AccountTeamPlayer,
            int fk_TeamPlayerType,
            int fk_Player,
            int fk_AccountTeam,
            int fk_Account,
            int fk_Season,
            int fk_GameWeak,
            bool? isPrimary,
            int GameWeakFrom,
            int GameWeakTo,
            bool? isTransfer)

        {
            return AccountTeamPlayerGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                                   (GameWeakFrom == 0 || a.GameWeak._365_GameWeakIdValue >= GameWeakFrom) &&
                                                   (GameWeakTo == 0 || a.GameWeak._365_GameWeakIdValue <= GameWeakTo) &&
                                                   (fk_AccountTeamPlayer == 0 || a.Fk_AccountTeamPlayer == fk_AccountTeamPlayer) &&
                                                   (fk_Account == 0 || a.AccountTeamPlayer.AccountTeam.Fk_Account == fk_Account) &&
                                                   (fk_Season == 0 || a.GameWeak.Fk_Season == fk_Season) &&
                                                   (fk_GameWeak == 0 || a.Fk_GameWeak == fk_GameWeak) &&
                                                   (isPrimary == null || a.IsPrimary == isPrimary) &&
                                                   (isTransfer == null || a.IsTransfer == isTransfer) &&
                                                   (fk_Player == 0 || a.AccountTeamPlayer.Fk_Player == fk_Player) &&
                                                   (fk_AccountTeam == 0 || a.AccountTeamPlayer.Fk_AccountTeam == fk_AccountTeam) &&
                                                   (fk_TeamPlayerType == 0 || a.Fk_TeamPlayerType == fk_TeamPlayerType));


        }

    }
}
