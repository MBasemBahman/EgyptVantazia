using Entities.CoreServicesModels.SponsorModels;
using Entities.DBModels.SponsorModels;
using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Repository.DBModels.SponsorModels
{
    public class SponsorRepository : RepositoryBase<Sponsor>
    {
        public SponsorRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Sponsor> FindAll(SponsorParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.AppViewEnum);
        }

        public async Task<Sponsor> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.SponsorLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(Sponsor entity)
        {
            entity.SponsorLang ??= new SponsorLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class SponsorRepositoryExtension
    {
        public static IQueryable<Sponsor> Filter(
            this IQueryable<Sponsor> Sponsors,
            int id,
            AppViewEnum? appViewEnum)
        {
            return Sponsors.Where(a => (id == 0 || a.Id == id) &&
                                       (appViewEnum == null || a.SponsorViews.Any(b => b.AppViewEnum == appViewEnum)));
        }

    }
}
