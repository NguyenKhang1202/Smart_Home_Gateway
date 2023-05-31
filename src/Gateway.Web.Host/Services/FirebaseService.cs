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
                Credential = GoogleCredential.FromFile("Services/notification-smart-social-firebase-adminsdk-3h5yi-26540eff7d.json"),
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
        public async Task<string> SendNotification(string Token, string Title, string Body)
        {
            try
            {
                string messageId = await _firebaseMessagingInstance
                    .SendAsync(CreateNotification(Title, Body, Token)).ConfigureAwait(false);
                return messageId;
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "";
            }
        }
    }
}
