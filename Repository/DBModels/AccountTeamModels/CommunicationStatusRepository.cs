using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.AccountTeamModels
{
    public class CommunicationStatusRepository : RepositoryBase<CommunicationStatus>
    {
        public CommunicationStatusRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<CommunicationStatus> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<CommunicationStatus> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(CommunicationStatus entity)
        {
            base.Create(entity);
        }
    }

    public static class CommunicationStatusRepositoryExtension
    {
        public static IQueryable<CommunicationStatus> Filter(this IQueryable<CommunicationStatus> CommunicationStatuss, int id)
        {
            return CommunicationStatuss.Where(a => id == 0 || a.Id == id);
        }

    }
}
