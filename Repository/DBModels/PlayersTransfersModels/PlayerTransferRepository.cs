using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.DBModels.PlayersTransfersModels;
using Entities.EnumData;


namespace Repository.DBModels.PlayersTransfersModels
{
    public class PlayerTransferRepository : RepositoryBase<PlayerTransfer>
    {
        public PlayerTransferRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PlayerTransfer> FindAll(PlayerTransferParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Player,
                           parameters.Fk_AccountTeam,
                           parameters.Fk_Team,
                           parameters.Fk_GameWeak,
                           parameters.Fk_Account,
                           parameters.Fk_Season,
                           parameters.IsFree,
                           parameters.TransferTypeEnum);
        }

        public async Task<PlayerTransfer> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }
    }

    public static class PlayerTransferRepositoryExtension
    {
        public static IQueryable<PlayerTransfer> Filter(
            this IQueryable<PlayerTransfer> PlayerTransfers,
            int id,
            int Fk_Player,
            int Fk_AccountTeam,
            int Fk_Team,
            int Fk_GameWeak,
            int Fk_Account,
            int Fk_Season,
            bool? IsFree,
            LogicEnumData.TransferTypeEnum? transferTypeEnum)
        {
            return PlayerTransfers.Where(a => (id == 0 || a.Id == id) &&
                                              (Fk_AccountTeam == 0 || a.Fk_AccountTeam == Fk_AccountTeam) &&
                                              
                                              (transferTypeEnum == null || a.TransferTypeEnum == transferTypeEnum) &&
                                              
                                              (Fk_Account == 0 || a.AccountTeam.Fk_Account == Fk_Account) &&
                                              
                                              (Fk_Team == 0 || a.Player.Fk_Team == Fk_Team) &&
                                              (Fk_Season == 0 || a.AccountTeam.Fk_Season == Fk_Season) &&
                                              (IsFree == null || a.IsFree == IsFree) &&
                                              (Fk_Player == 0 || a.Fk_Player == Fk_Player) &&
                                              (Fk_GameWeak == 0 || a.Fk_GameWeak == Fk_GameWeak));

        }

    }
}
