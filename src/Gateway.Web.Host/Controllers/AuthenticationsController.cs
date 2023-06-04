using AutoMapper;
using Gateway.Core.Dtos;
using Gateway.Core.Dtos.Authentications;
using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Authentications;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using static Gateway.Web.Host.Protos.Users.UserGrpc;

namespace Gateway.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly AuthenticationGrpc.AuthenticationGrpcClient _authenticationGrpcClient;
        private readonly IAppSession _appSession;
        private readonly IMapper _mapper;
        public AuthenticationsController(
            AuthenticationGrpc.AuthenticationGrpcClient authenticationGrpcClient,
            IAppSession appSession,
            IMapper mapper) 
        {
            _authenticationGrpcClient = authenticationGrpcClient;
            _appSession = appSession;
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
                    Data = response,
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
        [Authorize]
        [HttpPost("refreshToken")]
        public async Task<ResponseDto> RefreshToken([FromBody] RefreshTokenInputDto input)
        {
            try
            {
                Protos.Authentications.RefreshTokenRequest request = new()
                {
                    RefreshToken = input.RefreshToken,
                    UserId = _appSession.GetUserId(),
                };
                RefreshTokenResponse response = await _authenticationGrpcClient.RefreshTokenAsync(request);
                return new ResponseDto()
                {
                    Data = response,
                    Success = true,
                    Message = "Refresh token success!"
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
        [Authorize]
        [HttpPost("logout")]
        public async Task<ResponseDto> Logout([FromBody] LogoutInputDto input)
        {
            try
            {
                LogoutRequest request = new()
                {
                    UserId = _appSession.GetUserId(),
                    FCMToken = input.FcmToken ?? "",
                };
                LogoutResponse response = await _authenticationGrpcClient.LogoutAsync(request);
                return new ResponseDto()
                {
                    Data = response,
                    Success = true,
                    Message = "Logout success!"
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
        [Authorize]
        [HttpPost("change-password")]
        public async Task<ResponseDto> ChangePassword([FromBody] ChangePasswordInputDto input)
        {
            try
            {
                ChangePasswordRequest request = _mapper.Map<ChangePasswordRequest>(input);
                request.UserId = _appSession.GetUserId();
                ChangePasswordResponse response = await _authenticationGrpcClient.ChangePasswordAsync(request);
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Change password success"
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
