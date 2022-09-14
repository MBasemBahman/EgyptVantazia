using Entities.CoreServicesModels.PlayersTransfersModels;
using Entities.DBModels.PlayersTransfersModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.PlayersTransfersModels
{
    public class PlayerTransferRepository : RepositoryBase<PlayerTransfer>
    {
        public PlayerTransferRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerTransfer> FindAll(PlayerTransferParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Player,
                           parameters.Fk_AccountTeam,
                           parameters.Fk_GameWeak);
        }

        public async Task<PlayerTransfer> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(PlayerTransfer entity)
        {
            base.Create(entity);
        }

        public new void Delete(PlayerTransfer entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class PlayerTransferRepositoryExtension
    {
        public static IQueryable<PlayerTransfer> Filter(
            this IQueryable<PlayerTransfer> PlayerTransfers,
            int id,
            int Fk_Player,
            int Fk_AccountTeam,
            int Fk_GameWeak)
        {
            return PlayerTransfers.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_AccountTeam == 0 || a.Fk_AccountTeam == Fk_AccountTeam) &&
                                                   (Fk_Player == 0 || a.Fk_Player == Fk_Player) &&
                                                   (Fk_GameWeak == 0 || a.Fk_GameWeak == Fk_GameWeak));

        }

    }
}
