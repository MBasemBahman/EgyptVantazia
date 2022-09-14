using Entities.DBModels.PlayersTransfersModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.PlayersTransfersModels
{
    public class PlayerTransferRepository : RepositoryBase<PlayerTransfer>
    {
        public PlayerTransferRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerTransfer> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
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
        public static IQueryable<PlayerTransfer> Filter(this IQueryable<PlayerTransfer> PlayerTransfers, int id)
        {
            return PlayerTransfers.Where(a => id == 0 || a.Id == id);
        }

    }
}
