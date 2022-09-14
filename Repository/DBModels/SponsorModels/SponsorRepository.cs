using Entities.DBModels.SponsorModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.SponsorModels
{
    public class SponsorRepository : RepositoryBase<Sponsor>
    {
        public SponsorRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Sponsor> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
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
        public static IQueryable<Sponsor> Filter(this IQueryable<Sponsor> Sponsors, int id)
        {
            return Sponsors.Where(a => id == 0 || a.Id == id);
        }

    }
}
