using Entities.DBModels.SponsorModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.SponsorModels
{
    public class SponsorViewRepository : RepositoryBase<SponsorView>
    {
        public SponsorViewRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<SponsorView> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<SponsorView> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(SponsorView entity)
        {
            base.Create(entity);
        }

        public new void Delete(SponsorView entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class SponsorViewRepositoryExtension
    {
        public static IQueryable<SponsorView> Filter(this IQueryable<SponsorView> SponsorViews, int id)
        {
            return SponsorViews.Where(a => id == 0 || a.Id == id);
        }

    }
}
