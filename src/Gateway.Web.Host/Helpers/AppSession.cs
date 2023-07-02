using Gateway.Web.Host.Protos.Authentications;

namespace Gateway.Web.Host.Helpers
{
    public interface IAppSession
    {
        PUserInfo GetCurrentUser();
        string GetUserId();
    }
    public class AppSession : IAppSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public PUserInfo GetCurrentUser()
        {
            return (PUserInfo)_httpContextAccessor.HttpContext!.Items["User"]!;
        }
        public string GetUserId()
        {
            PUserInfo? user = (PUserInfo)_httpContextAccessor.HttpContext!.Items["User"]!;
            if (user != null) return user.UserId; else return "";
        }
    }
}
