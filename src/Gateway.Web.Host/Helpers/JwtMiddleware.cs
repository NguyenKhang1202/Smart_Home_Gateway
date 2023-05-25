using Gateway.Web.Host.Protos.Authentications;
using Gateway.Web.Host.Protos.Users;

namespace Gateway.Web.Host.Helpers
{
    public class JwtMiddleware : IMiddleware
    {
        private readonly AuthenticationGrpc.AuthenticationGrpcClient _authenticationGrpcClient;
        private readonly UserGrpc.UserGrpcClient _userGrpcClient;
        public JwtMiddleware(
            AuthenticationGrpc.AuthenticationGrpcClient authenticationGrpcClient,
            UserGrpc.UserGrpcClient userGrpcClient)
        {
            _authenticationGrpcClient = authenticationGrpcClient; 
            _userGrpcClient = userGrpcClient;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
                AttachUserToContext(context, token);

            await next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                ValidateTokenResponse response = _authenticationGrpcClient.ValidateToken(new ValidateTokenRequest()
                {
                    Token = token
                });

                // attach user to context on successful jwt validation
                PUserInfo userInfo = response.Data;
                context.Items["User"] = userInfo;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
