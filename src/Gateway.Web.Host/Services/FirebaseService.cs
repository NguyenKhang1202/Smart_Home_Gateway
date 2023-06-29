using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace Gateway.Web.Host.Services
{
    public interface IFirebaseService
    {
        Task<string> SendNotification(string Token, string Title, string Body);
    }
    public class FirebaseService : IFirebaseService
    {
        private FirebaseMessaging _firebaseMessagingInstance { get; set; }
        private FirebaseApp _app { get; set; }
        public FirebaseService()
        {
            _app = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("Services/fire-alarm-system-c6a01-firebase-adminsdk-yxd76-4fbc8e1a08.json"),
            });
            _firebaseMessagingInstance = FirebaseMessaging.GetMessaging(_app);
        }
        private Message CreateNotification(string title, string notificationBody, string token)
        {
            return new Message()
            {
                Token = token,
                Notification = new Notification()
                {
                    Body = notificationBody,
                    Title = title
                }
            };
        }
        public async Task<string> SendNotification(string token, string title, string body)
        {
            try
            {
                string messageId = await _firebaseMessagingInstance
                    .SendAsync(CreateNotification(title, body, token)).ConfigureAwait(false);
                return messageId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "";
            }
        }
    }
}
