namespace Gateway.Web.Host.Helpers
{
    public class JwtMiddleware : IMiddleware
    {
        private readonly AuthenticationGrpc.AuthenticationGrpcClient _authenticationGrpcClient;

        public JwtMiddleware(
            AuthenticationGrpc.AuthenticationGrpcClient authenticationGrpcClient)
        {
            _authenticationGrpcClient = authenticationGrpcClient; 
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
                context.Items["User"] = response.Data;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
