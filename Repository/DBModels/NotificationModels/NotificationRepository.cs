using Entities.CoreServicesModels.NotificationModels;
using Entities.DBModels.NotificationModels;
using static Entities.EnumData.LogicEnumData;

namespace Repository.DBModels.NotificationModels
{
    public class NotificationRepository : RepositoryBase<Notification>
    {
        public NotificationRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<Notification> FindAll(NotificationParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.OpenTypes);
        }

        public async Task<Notification> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.NotificationLang)
                        .FirstOrDefaultAsync();
        }

        public new void Create(Notification entity)
        {
            if (FindByCondition(a => a.Title == entity.Title &&
                                     a.Description == entity.Description &&
                                     a.OpenValue == entity.OpenValue &&
                                     a.OpenType == entity.OpenType, trackChanges: false).Any())
            {
                return;
            }

            entity.NotificationLang ??= new NotificationLang
            {
                Title = entity.Title,
                Description = entity.Description,

            };
            base.Create(entity);
        }
    }

    public static class NotificationRepositoryExtension
    {
        public static IQueryable<Notification> Filter(
            this IQueryable<Notification> Notifications,
            int id,
            List<NotificationOpenTypeEnum> openTypes)
        {
            DateTime now = DateTime.UtcNow;
            return Notifications.Where(a => (id == 0 || a.Id == id) &&
                                            (openTypes == null || !openTypes.Any() ||
                                             openTypes.Contains(a.OpenType)));
        }

    }
}
