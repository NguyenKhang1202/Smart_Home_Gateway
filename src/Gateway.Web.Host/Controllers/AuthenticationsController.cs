using AutoMapper;
using Gateway.Core.Dtos;
using Gateway.Core.Dtos.Authentications;
using Gateway.Web.Host.Protos.Authentications;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly AuthenticationGrpc.AuthenticationGrpcClient _authenticationGrpcClient;
        private readonly IMapper _mapper;
        public AuthenticationsController(
            AuthenticationGrpc.AuthenticationGrpcClient authenticationGrpcClient,
            IMapper mapper) 
        {
            _authenticationGrpcClient = authenticationGrpcClient;
            _mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<ResponseDto> Login([FromBody] LoginInputDto input)
        {
            try
            {
                LoginResponse response = await _authenticationGrpcClient.LoginAsync(_mapper.Map<LoginRequest>(input));
                return new ResponseDto()
                {
                    Data = response.Token,
                    Success = true,
                    Message = "Login success!"
                };
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

        [HttpPost("register")]
        public async Task<ResponseDto> Register([FromBody] RegisterInputDto input)
        {
            try
            {
                RegisterResponse response = await _authenticationGrpcClient.RegisterAsync(_mapper.Map<RegisterRequest>(input));
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Register success!"
                };
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
