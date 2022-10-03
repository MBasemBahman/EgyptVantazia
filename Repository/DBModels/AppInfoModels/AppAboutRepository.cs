using Entities.DBModels.AppInfoModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.AppInfoModels
{
    public class AppAboutRepository : RepositoryBase<AppAbout>
    {
        public AppAboutRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AppAbout> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<AppAbout> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.AppAboutLang)
                        .SingleOrDefaultAsync();
        }

        public async Task<AppAbout> Find(bool trackChanges)
        {
            return await FindByCondition(a => true, trackChanges)
                        .Include(a => a.AppAboutLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(AppAbout entity)
        {
            base.Create(entity);
        }

        public new void Delete(AppAbout entity)
        {
            entity.AppAboutLang ??= new AppAboutLang
            {
                AboutCompany = entity.AboutCompany,
                AboutApp = entity.AboutApp,
                TermsAndConditions = entity.TermsAndConditions,
                QuestionsAndAnswer = entity.QuestionsAndAnswer,
                GameRules = entity.GameRules,
                Subscriptions = entity.Subscriptions,
                Prizes = entity.Prizes,
            };
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class AppAboutRepositoryExtension
    {
        public static IQueryable<AppAbout> Filter(this IQueryable<AppAbout> AppAbouts, int id)
        {
            return AppAbouts.Where(a => id == 0 || a.Id == id);
        }

    }
}
