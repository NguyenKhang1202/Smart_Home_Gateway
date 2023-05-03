using AutoMapper;
using Gateway.Core.Dtos.Authentications;
using Gateway.Web.Host.Helpers;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Web.Host.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthenticationGrpc.AuthenticationGrpcClient _authenticationGrpcClient;
        private readonly IMapper _mapper;
        public UsersController(
            AuthenticationGrpc.AuthenticationGrpcClient authenticationGrpcClient,
            IMapper mapper
            ) 
        {
            _authenticationGrpcClient = authenticationGrpcClient;
            _mapper = mapper;
        }

        [HttpGet("")]
        public object GetAllUsers()
        {
            try
            {
                return "1";
            }
            catch (RpcException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
