
using Entities.CoreServicesModels.NotificationModels;
using Entities.DBModels.NotificationModels;

namespace CoreServices.Logic
{
    public class NotificationServices
    {
        private readonly RepositoryManager _repository;

        public NotificationServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region Notification Services
        public IQueryable<NotificationModel> GetNotifications(NotificationParameters parameters,
                bool otherLang)
        {
            return _repository.Notification
                       .FindAll(parameters, trackChanges: false)
                       .Select(x => new NotificationModel
                       {
                           Id = x.Id,
                           CreatedAt = x.CreatedAt,
                           CreatedBy = x.CreatedBy,
                           LastModifiedAt = x.LastModifiedAt,
                           LastModifiedBy = x.LastModifiedBy,
                           Title = otherLang ? x.NotificationLang.Title : x.Title,
                           Description = otherLang ? x.NotificationLang.Description : x.Description,
                           OpenType = x.OpenType,
                           ShowAt = x.ShowAt,
                           ExpireAt = x.ExpireAt,
                           ImageUrl = x.StorageUrl + x.ImageUrl,
                           OpenValue = x.OpenValue,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<NotificationModel>> GetNotificationPaged(
                  NotificationParameters parameters,
                  bool otherLang)
        {
            return await PagedList<NotificationModel>.ToPagedList(GetNotifications(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Notification> FindNotificationbyId(int id, bool trackChanges)
        {
            return await _repository.Notification.FindById(id, trackChanges);
        }

        public void CreateNotification(Notification Notification)
        {
            _repository.Notification.Create(Notification);
        }

        public async Task DeleteNotification(int id)
        {
            Notification Notification = await FindNotificationbyId(id, trackChanges: true);
            _repository.Notification.Delete(Notification);
        }

        public NotificationModel GetNotificationbyId(int id, bool otherLang)
        {
            return GetNotifications(new NotificationParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetNotificationCount()
        {
            return _repository.Notification.Count();
        }
        #endregion
    }
}
