using Entities.CoreServicesModels.UserModels;

namespace Repository.DBModels.UserModels
{
    public class DeviceRepository : RepositoryBase<Device>
    {
        public DeviceRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<Device> FindAll(
          UserDeviceParameters parameters,
          bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Fk_User);

        }

        public Device FindByNotificationToken(string notificationToken, bool trackChanges)
        {
            if (string.IsNullOrWhiteSpace(notificationToken))
            {
                return null;
            }

            notificationToken = notificationToken.SafeLower().SafeTrim();

            return FindByCondition(a => a.NotificationToken.ToLower() == notificationToken, trackChanges).SingleOrDefault();
        }

        public async Task<IEnumerable<Device>> FindDevicesByUserId(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Fk_User == id, trackChanges)
                        .ToListAsync();
        }
    }

    public static class UserDeviceRepositoryExtensions
    {
        public static IQueryable<Device> Filter(
            this IQueryable<Device> devices,
            int fk_User)
        {
            return devices.Where(a => fk_User == 0 || a.Fk_User == fk_User);
        }
    }
}
