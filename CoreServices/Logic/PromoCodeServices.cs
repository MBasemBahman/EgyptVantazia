using Entities.CoreServicesModels.PromoCodeModels;
using Entities.DBModels.PromoCodeModels;

namespace CoreServices.Logic
{
    public class PromoCodeServices
    {
        private readonly RepositoryManager _repository;

        public PromoCodeServices(RepositoryManager repository)
        {
            _repository = repository;
        }
        #region PromoCode Services
        public IQueryable<PromoCodeModel> GetPromoCodes(PromoCodeParameters parameters, bool otherLang)
        {
            return _repository.PromoCode
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PromoCodeModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Name = otherLang ? a.PromoCodeLang.Name : a.Name,
                           Description = otherLang ? a.PromoCodeLang.Description : a.Description,
                           IsActive = a.IsActive,
                           Code = a.Code,
                           Discount = a.Discount,
                           ExpirationDate = a.ExpirationDate,
                           MaxDiscount = a.MaxDiscount,
                           MaxUse = a.MaxUse,
                           MaxUsePerUser = a.MaxUsePerUser,
                           UsedCount = a.AccountSubscriptions.Count,
                           UserUsedCount = a.AccountSubscriptions.Count(a => a.Fk_Account == parameters.Fk_Account),
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<PromoCodeModel>> GetPromoCodesPaged(
                  PromoCodeParameters parameters, bool otherLang)
        {
            return await PagedList<PromoCodeModel>.ToPagedList(GetPromoCodes(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public Dictionary<string, string> GetPromoCodeLookUp(PromoCodeParameters parameters, bool otherLang)
        {
            return GetPromoCodes(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }

        public async Task<PromoCode> FindPromoCodebyId(int id, bool trackChanges)
        {
            return await _repository.PromoCode.FindById(id, trackChanges);
        }

        public void CreatePromoCode(PromoCode entiy)
        {
            _repository.PromoCode.Create(entiy);
        }

        public async Task DeletePromoCode(int id)
        {
            PromoCode entity = await FindPromoCodebyId(id, trackChanges: true);
            _repository.PromoCode.Delete(entity);
        }

        public PromoCodeModel GetPromoCodebyId(int id, bool otherLang)
        {
            return GetPromoCodes(new PromoCodeParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetPromoCodeCount()
        {
            return _repository.PromoCode.Count();
        }

        public PromoCodeModel CheckPromoCode(string code, int fk_Subscription, int fk_Account, bool otherLang)
        {
            PromoCodeModel data = GetPromoCodes(new PromoCodeParameters
            {
                Code = code,
                Fk_Subscription = fk_Subscription,
            }, otherLang).FirstOrDefault() ?? throw new Exception("Invalid code!");

            if (!data.IsValid)
            {
                if (data.IsExpired)
                {
                    throw new Exception("The discount code has expired!");
                }
                else if (data.IsMaxReach)
                {
                    throw new Exception("The maximum usage limit has been reached!");
                }
                else if (data.IsMaxReachPerUser)
                {
                    throw new Exception("Your account usage limit has been reached!");
                }
                else
                {
                    throw new Exception("Invalid code!");
                }
            }

            int price = _repository.Subscription
                                   .FindByCondition(a => a.Id == fk_Subscription, trackChanges: false)
                                   .Select(a => a.CostAfterDiscount)
                                   .FirstOrDefault();

            return GetDiscountAmount(data, price);
        }

        public PromoCodeModel GetDiscountAmount(PromoCodeModel promoCode, double price)
        {
            double discountAmount = price / 100 * promoCode.Discount;

            promoCode.DiscountAmount = (promoCode.MaxDiscount > 0 && discountAmount > promoCode.MaxDiscount) ? promoCode.MaxDiscount.Value : discountAmount;

            return promoCode;
        }

        #endregion

        #region PromoCodeSubscription Services
        public IQueryable<PromoCodeSubscriptionModel> GetPromoCodeSubscriptions(PromoCodeSubscriptionParameters parameters, bool otherLang)
        {
            return _repository.PromoCodeSubscription
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PromoCodeSubscriptionModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           Fk_PromoCode = a.Fk_PromoCode,
                           Fk_Subscription = a.Fk_Subscription,
                           PromoCode = new PromoCodeModel
                           {
                               Id = a.Fk_PromoCode,
                               Name = otherLang ? a.PromoCode.PromoCodeLang.Name : a.PromoCode.Name
                           },
                           Subscription = new Entities.CoreServicesModels.SubscriptionModels.SubscriptionModel
                           {
                               Id = a.Fk_Subscription,
                               Name = otherLang ? a.Subscription.SubscriptionLang.Name : a.Subscription.Name
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<PromoCodeSubscriptionModel>> GetPromoCodeSubscriptionPaged(
                  PromoCodeSubscriptionParameters parameters, bool otherLang)
        {
            return await PagedList<PromoCodeSubscriptionModel>.ToPagedList(GetPromoCodeSubscriptions(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PromoCodeSubscription> FindPromoCodeSubscriptionbyId(int id, bool trackChanges)
        {
            return await _repository.PromoCodeSubscription.FindById(id, trackChanges);
        }

        public void CreatePromoCodeSubscription(PromoCodeSubscription entity)
        {
            _repository.PromoCodeSubscription.Create(entity);
        }

        public async Task DeletePromoCodeSubscription(int id)
        {
            PromoCodeSubscription entity = await FindPromoCodeSubscriptionbyId(id, trackChanges: true);
            _repository.PromoCodeSubscription.Delete(entity);
        }

        public PromoCodeSubscriptionModel GetPromoCodeSubscriptionbyId(int id, bool otherLang)
        {
            return GetPromoCodeSubscriptions(new PromoCodeSubscriptionParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetPromoCodeSubscriptionount()
        {
            return _repository.PromoCodeSubscription.Count();
        }

        public void AddPromoCodeSubscriptions(int fk_PromoCode, List<int> fk_Subscriptions)
        {
            if (fk_Subscriptions != null && fk_Subscriptions.Any())
            {
                foreach (int fk_subscription in fk_Subscriptions)
                {
                    _repository.PromoCodeSubscription.Create(new()
                    {
                        Fk_PromoCode = fk_PromoCode,
                        Fk_Subscription = fk_subscription
                    });
                }
            }
        }

        public async Task DeletePromoCodeSubscriptions(List<int> fk_Subscriptions)
        {
            if (fk_Subscriptions.Count == 0)
            {
                return;
            }

            foreach (int fk_Subscription in fk_Subscriptions)
            {
                await DeletePromoCodeSubscription(fk_Subscription);
            }
        }

        public async Task UpdatePromoCodeSubscriptions(int fk_PromoCode, List<int> fk_Subscriptions)
        {
            fk_Subscriptions ??= new List<int>();
            List<int> oldData = GetPromoCodeSubscriptions(new PromoCodeSubscriptionParameters
            {
                Fk_PromoCode = fk_PromoCode,
            }, otherLang: false).Select(a => a.Fk_Subscription).ToList();


            List<int> dataToCreate = fk_Subscriptions.Except(oldData).ToList();

            List<int> dataToDelete = oldData.Except(fk_Subscriptions).ToList();


            AddPromoCodeSubscriptions(fk_PromoCode, dataToCreate);
            await DeletePromoCodeSubscriptions(dataToDelete);
        }
        #endregion
    }
}
