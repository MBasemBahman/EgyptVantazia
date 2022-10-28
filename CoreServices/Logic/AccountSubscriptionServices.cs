using Entities.CoreServicesModels.AccountModels;
using Entities.DBModels.AccountModels;

namespace CoreServices.Logic
{
    public class AccountSubscriptionServices
    {
        private readonly RepositoryManager _repository;

        public AccountSubscriptionServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region Account Subscription Services

        public IQueryable<AccountSubscriptionModel> GetAccountSubscriptions(
            AccountSubscriptionParameters parameters,
            bool otherLang)
        {
            return _repository.AccountSubscription
                              .FindAll(parameters, trackChanges: false)
                              .Select(a => new AccountSubscriptionModel
                              {
                                  Id = a.Id,
                                  CreatedAt = a.CreatedAt,
                                  StartDate = a.StartDate,
                                  EndDate = a.EndDate,
                                  IsAction = a.IsAction,
                                  Fk_Account = a.Fk_Account,
                                  Fk_Subscription = a.Fk_Subscription
                              })
                              .Search(parameters.SearchColumns, parameters.SearchTerm)
                              .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<AccountSubscriptionModel>> GetAccountSubscriptionsPaged(
            AccountSubscriptionParameters parameters,
            bool otherLang)
        {
            return await PagedList<AccountSubscriptionModel>.ToPagedList(GetAccountSubscriptions(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<AccountSubscriptionModel>> GetAccountSubscriptionsPaged(
          IQueryable<AccountSubscriptionModel> data,
         AccountSubscriptionParameters parameters)
        {
            return await PagedList<AccountSubscriptionModel>.ToPagedList(data, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<AccountSubscription> FindAccountSubscriptionById(int id, bool trackChanges)
        {
            return await _repository.AccountSubscription.FindById(id, trackChanges);
        }

        public AccountSubscriptionModel GetAccountSubscriptionbyId(int id, bool otherLang)
        {
            return GetAccountSubscriptions(new AccountSubscriptionParameters { Id = id }, otherLang).SingleOrDefault();
        }


        public void CreateAccountSubscription(AccountSubscription account)
        {
            _repository.AccountSubscription.Create(account);
        }

        public int GetAccountSubscriptionsCount()
        {
            return _repository.AccountSubscription.Count();
        }

        #endregion
    }
}
