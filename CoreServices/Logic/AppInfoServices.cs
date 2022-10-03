using Entities.CoreServicesModels.AppInfoModels;
using Entities.DBModels.AppInfoModels;

namespace CoreServices.Logic
{
    public class AppInfoServices
    {
        private readonly RepositoryManager _repository;

        public AppInfoServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region AppAbout Services
        public IQueryable<AppAboutModel> GetAppAbouts(RequestParameters parameters,
                bool otherLang)
        {
            return _repository.AppAbout
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new AppAboutModel
                       {
                           AboutCompany = otherLang ? a.AppAboutLang.AboutCompany : a.AboutCompany,
                           AboutApp = otherLang ? a.AppAboutLang.AboutApp : a.AboutApp,
                           TermsAndConditions = otherLang ? a.AppAboutLang.TermsAndConditions : a.TermsAndConditions,
                           QuestionsAndAnswer = otherLang ? a.AppAboutLang.QuestionsAndAnswer : a.QuestionsAndAnswer,
                           GameRules = otherLang ? a.AppAboutLang.GameRules : a.GameRules,
                           Subscriptions = otherLang ? a.AppAboutLang.Subscriptions : a.Subscriptions,
                           Prizes = otherLang ? a.AppAboutLang.Prizes : a.Prizes,
                           Phone = a.Phone,
                           WhatsApp = a.WhatsApp,
                           EmailAddress = a.EmailAddress,
                           TwitterUrl = a.TwitterUrl,
                           FacebookUrl = a.FacebookUrl,
                           InstagramUrl = a.InstagramUrl,
                           SnapChatUrl = a.SnapChatUrl,
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<AppAboutModel>> GetAppAboutsPaged(
                  RequestParameters parameters,
                  bool otherLang)
        {
            return await PagedList<AppAboutModel>.ToPagedList(GetAppAbouts(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<AppAbout> FindAppAboutbyId(int id, bool trackChanges)
        {
            return await _repository.AppAbout.FindById(id, trackChanges);
        }

        public async Task<AppAbout> FindAppAbout(bool trackChanges)
        {
            return await _repository.AppAbout.Find(trackChanges);
        }

        public void CreateAppAbout(AppAbout AppAbout)
        {
            _repository.AppAbout.Create(AppAbout);
        }

        public async Task DeleteAppAbout(int id)
        {
            AppAbout AppAbout = await FindAppAboutbyId(id, trackChanges: true);
            _repository.AppAbout.Delete(AppAbout);
        }

        public AppAboutModel GetAppAboutbyId(int id, bool otherLang)
        {
            return GetAppAbouts(new RequestParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetAppAboutCount()
        {
            return _repository.AppAbout.Count();
        }
        #endregion
    }
}
