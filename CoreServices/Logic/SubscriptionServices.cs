using Entities.CoreServicesModels.SubscriptionModels;
using Entities.DBModels.SubscriptionModels;

namespace CoreServices.Logic
{
    public class SubscriptionServices
    {
        private readonly RepositoryManager _repository;

        public SubscriptionServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region Subscription Services
        public IQueryable<SubscriptionModel> GetSubscriptions(SubscriptionParameters parameters,
                bool otherLang)
        {
            return _repository.Subscription
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new SubscriptionModel
                       {
                           Name = otherLang ? a.SubscriptionLang.Name : a.Name,
                           Description = otherLang ? a.SubscriptionLang.Description : a.Description,
                           ImageUrl = a.StorageUrl + a.ImageUrl,
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           ForAction = a.ForAction,
                           IsActive = a.IsActive,
                           Cost = a.Cost,
                           Discount = a.Discount,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<SubscriptionModel>> GetSubscriptionPaged(
                  SubscriptionParameters parameters,
                  bool otherLang)
        {
            return await PagedList<SubscriptionModel>.ToPagedList(GetSubscriptions(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public Dictionary<string, string> GetSubscriptionsLookUp(SubscriptionParameters parameters, bool otherLang)
        {
            return GetSubscriptions(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }

        public async Task<Subscription> FindSubscriptionById(int id, bool trackChanges)
        {
            return await _repository.Subscription.FindById(id, trackChanges);
        }

        public async Task<string> UploadSubscriptionImage(string rootPath, IFormFile file)
        {
            FileUploader uploader = new(rootPath);
            return await uploader.UploudFile(file, "Uploud/Subscription");
        }

        public void CreateSubscription(Subscription Subscription)
        {
            _repository.Subscription.Create(Subscription);
        }

        public async Task DeleteSubscription(int id)
        {
            Subscription Subscription = await FindSubscriptionById(id, trackChanges: true);
            _repository.Subscription.Delete(Subscription);
        }

        public SubscriptionModel GetSubscriptionById(int id, bool otherLang)
        {
            return GetSubscriptions(new SubscriptionParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetSubscriptionCount()
        {
            return _repository.Subscription.Count();
        }
        #endregion

    }
}
