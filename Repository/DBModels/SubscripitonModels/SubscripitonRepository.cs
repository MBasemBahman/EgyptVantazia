using Entities.CoreServicesModels.SubscriptionModels;
using Entities.DBModels.SubscriptionModels;

namespace Repository.DBModels.SubscriptionModels
{
    public class SubscriptionRepository : RepositoryBase<Subscription>
    {
        public SubscriptionRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Subscription> FindAll(SubscriptionParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<Subscription> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.SubscriptionLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(Subscription entity)
        {
            entity.SubscriptionLang ??= new SubscriptionLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class SubscriptionRepositoryExtension
    {
        public static IQueryable<Subscription> Filter(
            this IQueryable<Subscription> Subscriptions,
            int id)
        {
            return Subscriptions.Where(a => (id == 0 || a.Id == id));
        }

    }
}
