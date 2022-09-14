using Entities.ServicesModels;

namespace Contracts.Services
{
    public interface IFirebaseNotificationManager
    {
        Task<int> SubscribeToTopic(List<string> registrationTokens, string topic);
        Task<int> UnsubscribeFromTopic(List<string> registrationTokens, string topic);
        Task SendToTopic(FirebaseNotificationModel model);
        Task SendMulticast(FirebaseNotificationModel model);
    }
}
