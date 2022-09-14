using Entities.ServicesModels;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json;

namespace Services
{
    public class FirebaseNotificationManager : IFirebaseNotificationManager
    {
        private static async Task<string> SendToTopic(Message message)
        {
            // Send a message to the device corresponding to the provided
            // registration token.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            return response;
            // [END send_to_token]
        }

        private static async Task<int> SendMulticast(MulticastMessage message)
        {
            BatchResponse response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            // See the BatchResponse reference documentation
            // for the contents of response.
            return response.SuccessCount;
            // [END send_multicast]
        }

        private static Dictionary<string, string> GetBaseMessage(FirebaseNotificationModel model)
        {
            Dictionary<string, string> Extra = new()
            {
                { "OpenType", model.OpenType.ToString()},
                { "OpenValue", model.OpenValue.ToString()},
            };

            Dictionary<string, string> Data = new()
            {
                { "Extra", JsonConvert.SerializeObject(Extra) },
                { "Title", model.MessageHeading },
                { "Body", model.MessageContent },
                { "ImgUrl", model.ImgUrl },
            };
            return Data;
        }

        public async Task<int> SubscribeToTopic(List<string> registrationTokens, string topic)
        {
            // Subscribe the devices corresponding to the registration tokens to the
            // topic
            TopicManagementResponse response = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(
                registrationTokens, topic);
            // See the TopicManagementResponse reference documentation
            // for the contents of response.
            return response.SuccessCount;
            // [END subscribe_to_topic]
        }

        public async Task<int> UnsubscribeFromTopic(List<string> registrationTokens, string topic)
        {
            // Subscribe the devices corresponding to the registration tokens to the
            // topic
            TopicManagementResponse response = await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(
                registrationTokens, topic);
            // See the TopicManagementResponse reference documentation
            // for the contents of response.
            return response.SuccessCount;
            // [END subscribe_to_topic]
        }

        public async Task SendToTopic(FirebaseNotificationModel model)
        {
            // [START apns_message]
            Message message = new()
            {
                Topic = model.Topic,
                Data = GetBaseMessage(model),
                Apns = new ApnsConfig()
                {
                    Aps = new Aps()
                    {
                        Alert = new ApsAlert()
                        {
                            Title = model.MessageHeading,
                            Body = model.MessageContent,
                            LaunchImage = model.ImgUrl,
                        },
                        Sound = "notification.wav",
                        ContentAvailable = true
                    },
                    FcmOptions = new()
                    {
                        ImageUrl = model.ImgUrl
                    },
                    Headers = new Dictionary<string, string>
                    {
                        {"apns-priority", "5" },
                    }
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        Body = model.MessageContent,
                        Title = model.MessageHeading,
                        Icon = model.ImgUrl,
                        ImageUrl = model.ImgUrl,
                        Sound = "default",
                    }
                },
            };
            // [END apns_message]

            _ = await SendToTopic(message);
        }

        public async Task SendMulticast(FirebaseNotificationModel model)
        {
            // [START apns_message]
            MulticastMessage message = new()
            {
                Tokens = model.RegistrationTokens.Where(a => !string.IsNullOrEmpty(a)).ToList(),
                Notification = new Notification()
                {
                    ImageUrl = model.ImgUrl,
                    Title = model.MessageHeading,
                    Body = model.MessageContent
                },
                Apns = new ApnsConfig()
                {
                    Aps = new Aps()
                    {
                        Alert = new ApsAlert()
                        {
                            Title = model.MessageHeading,
                            Body = model.MessageContent,
                            LaunchImage = model.ImgUrl
                        },
                        Sound = "notification.wav",
                        ContentAvailable = true
                    },
                    FcmOptions = new()
                    {
                        ImageUrl = model.ImgUrl
                    },
                    Headers = new Dictionary<string, string>
                    {
                        {"apns-priority", "5" },
                        {"apns-push-type", "background" }
                    },
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        Body = model.MessageContent,
                        Title = model.MessageHeading,
                        Icon = model.ImgUrl,
                        ImageUrl = model.ImgUrl,
                        Sound = "notification"
                    }
                },
                Data = GetBaseMessage(model),
            };
            // [END apns_message]

            _ = await SendMulticast(message);
        }
    }
}
