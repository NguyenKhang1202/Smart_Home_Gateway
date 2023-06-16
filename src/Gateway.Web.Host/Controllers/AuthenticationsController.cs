using AutoMapper;
using Gateway.Core.Dtos;
using Gateway.Core.Dtos.Authentications;
using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Authentications;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Login([FromBody] LoginInputDto input)
        {
            try
            {
                LoginResponse response = _authenticationGrpcClient.Login(_mapper.Map<LoginRequest>(input));
                return Ok(new ResponseDto()
                {
                    Data = response,
                    Success = true,
                    Message = "Login success!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Login fail"
                });
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterInputDto input)
        {
            try
            {
                RegisterResponse response = _authenticationGrpcClient.Register(_mapper.Map<RegisterRequest>(input));
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Register success!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Register fail!"
                });
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
            catch (Exception ex)
            {
                return new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Logout fail!"
                };
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
            catch (Exception ex)
            {
                return new ResponseDto()
                {
                    Data = ex.Message,
                    Success = false,
                    Message = "Change password fail"
                };
            }
        }

        [HttpPost("verify-token")]
        public async Task<ResponseDto> VerifyToken([FromBody] VerifyTokenInputDto input)
        {
            try
            {
                ValidateTokenRequest request = _mapper.Map<ValidateTokenRequest>(input);
                ValidateTokenResponse response = await _authenticationGrpcClient.ValidateTokenAsync(request);
                return new ResponseDto()
                {
                    Data = true,
                    Success = true,
                    Message = "Verify token success"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto()
                {
                    Data = ex.Message,
                    Success = false,
                    Message = "Token is invalid"
                };
            }
        }
    }
}
