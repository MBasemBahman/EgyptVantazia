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
    public class PromoCodeRepository : RepositoryBase<PromoCode>
    {
        public PromoCodeRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PromoCode> FindAll(PromoCodeParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.IsActive,
                           parameters.Code,
                           parameters.Fk_Subscription);
        }

        public async Task<PromoCode> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.PromoCodeLang)
                        .FirstOrDefaultAsync();
        }
        public new void Create(PromoCode entity)
        {
            entity.PromoCodeLang ??= new PromoCodeLang
            {
                Name = entity.Name,
                Description = entity.Description
            };
            base.Create(entity);
        }
    }

    public static class PromoCodeRepositoryExtension
    {
        public static IQueryable<PromoCode> Filter(
            this IQueryable<PromoCode> data,
            int id,
            bool? isActive,
            string code, 
            int fk_Subscription)
        {
            return data.Where(a => (id == 0 || a.Id == id) &&
                                   (isActive == null || a.IsActive == isActive) &&
                                   (string.IsNullOrEmpty(code) || a.Code.ToLower() == code.ToLower()) &&
                                   (fk_Subscription == 0 || !a.PromoCodeSubscriptions.Any() || a.PromoCodeSubscriptions.Any(b => b.Fk_Subscription == fk_Subscription)));

        }

    }
}
