using AutoMapper;
using Gateway.Core.Dtos;
using Gateway.Core.Dtos.Users;
using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Users;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Web.Host.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserGrpc.UserGrpcClient _userGrpcClient;
        private readonly IAppSession _appSession;
        private readonly IMapper _mapper;
        public UsersController(
            UserGrpc.UserGrpcClient userGrpcClient,
            IAppSession appSession,
            IMapper mapper
            )
        {
            _userGrpcClient = userGrpcClient;
            _appSession = appSession;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersInputDto input)
        {
            try
            {
                GetAllUsersResponse response = await _userGrpcClient.GetAllUsersAsync(
                    _mapper.Map<GetAllUsersRequest>(input));
                return Ok(new ResponseDto()
                {
                    TotalCount = response.TotalCount,
                    Data = response.Items,
                    Success = true,
                    Message = "Get users success"
                });
            }
            catch (RpcException ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = ex.Status.Detail
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Get users failed"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                GetUserByIdResponse response = await _userGrpcClient.GetUserByIdAsync(
                    new GetUserByIdRequest()
                    {
                        Id = id,
                    });
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Get user success"
                });
            }
            catch (RpcException ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = ex.Status.Detail
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Get user failed"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserInputDto input)
        {
            try
            {
                UpdateUserResponse response = await _userGrpcClient.UpdateUserAsync(
                    _mapper.Map<UpdateUserRequest>(input));
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Update user success"
                });
            }
            catch (RpcException ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = ex.Status.Detail
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Update user failed"
                });
            }
        }

        [HttpPost("fcmToken")]
        public async Task<IActionResult> SaveFcmToken([FromBody] SaveFcmTokenInputDto input)
        {
            try
            {
                SaveFcmTokenRequest request = new()
                {
                    UserId = _appSession.GetUserId(),
                    FCMToken = input.FcmToken ?? "",
                };
                SaveFcmTokenResponse response = await _userGrpcClient.SaveFcmTokenAsync(request);
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Save FcmToken success"
                });
            }
            catch (RpcException ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = ex.Status.Detail
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Save FcmToken failed"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                DeleteUserResponse response = await _userGrpcClient.DeleteUserAsync(
                    new DeleteUserRequest()
                    {
                        Id = id
                    });
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Delete user success"
                });
            }
            catch (RpcException ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = ex.Status.Detail
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Delete user failed"
                });
            }
        }
    }
}
