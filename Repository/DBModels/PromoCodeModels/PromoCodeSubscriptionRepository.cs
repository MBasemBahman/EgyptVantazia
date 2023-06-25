using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.CoreServicesModels.PromoCodeModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.DBModels.PromoCodeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DBModels.PromoCodeModels
{
    public class PromoCodeSubscriptionRepository : RepositoryBase<PromoCodeSubscription>
    {
        public PromoCodeSubscriptionRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PromoCodeSubscription> FindAll(PromoCodeSubscriptionParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Subscription,
                           parameters.Fk_PromoCode);
        }

        public async Task<PromoCodeSubscription> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }


        public new void Create(PromoCodeSubscription entity)
        {
            base.Create(entity);
        }
    }

    public static class PromoCodeSubscriptionRepositoryExtension
    {
        public static IQueryable<PromoCodeSubscription> Filter(
            this IQueryable<PromoCodeSubscription> data,
            int id,
            int fk_Subscription,
            int fk_PromoCode)
        {
            return data.Where(a => (id == 0 || a.Id == id) &&
                                            (fk_PromoCode == 0 || a.Fk_PromoCode == fk_PromoCode) &&
                                            (fk_Subscription == 0 || a.Fk_Subscription == fk_Subscription));

        }
    }
    }
