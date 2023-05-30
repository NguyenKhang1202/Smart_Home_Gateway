using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace Gateway.Web.Host.Services
{
    public interface IFirebaseService
    {
        Task<string> SendFcmMessage(Message message);
    }
    public class FirebaseService : IFirebaseService
    {
        private FirebaseMessaging _firebaseMessagingInstance { get; set; }
        private FirebaseApp _app { get; set; }
        public FirebaseService() 
        {
            _app = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("src/Gateway.Web.Host/Services/notification-smart-social-firebase-adminsdk-3h5yi-26540eff7d.json"),
            });
            _firebaseMessagingInstance = FirebaseMessaging.GetMessaging(_app);
        }

        public async Task<string> SendFcmMessage(Message message)
        {
            try
            {
                string messageId = await _firebaseMessagingInstance.SendAsync(message).ConfigureAwait(false);
                return messageId;
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "";
            }
        }
    }
}
