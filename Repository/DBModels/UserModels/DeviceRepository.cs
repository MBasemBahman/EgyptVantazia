using Entities.CoreServicesModels.UserModels;

namespace Repository.DBModels.UserModels
{
    public class DeviceRepository : RepositoryBase<Device>
    {
        public DeviceRepository(DbContext context) : base(context)
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

        public void CreateDevice(Device device)
        {
            Create(device);
        }

        public void DeleteDevice(Device device)
        {
            Delete(device);
        }

        public async Task<IEnumerable<Device>> FindDevicesByUserId(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Fk_User == id, trackChanges)
                        .ToListAsync();
        }

        public new int Count()
        {
            return base.Count();
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
