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
        private readonly IMapper _mapper;
        public UsersController(
            UserGrpc.UserGrpcClient userGrpcClient,
            IMapper mapper
            ) 
        {
            _userGrpcClient = userGrpcClient;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ResponseDto> GetAllUsers([FromQuery] GetAllUsersInputDto input)
        {
            try
            {
                GetAllUsersResponse response = await _userGrpcClient.GetAllUsersAsync(
                    _mapper.Map<GetAllUsersRequest>(input));
                return new ResponseDto()
                {
                    TotalCount = response.TotalCount,
                    Data = response.Items,
                    Success = true,
                    Message = "Get users success"
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

        [HttpGet("{id}")]
        public async Task<ResponseDto> GetUserById(string id)
        {
            try
            {
                GetUserByIdResponse response = await _userGrpcClient.GetUserByIdAsync(
                    new GetUserByIdRequest()
                    {
                        Id = id,
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Get user success"
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

        [HttpPut("{id}")]
        public async Task<ResponseDto> UpdateUser(string id, [FromBody] UpdateUserInputDto input)
        {
            try
            {
                UpdateUserResponse response = await _userGrpcClient.UpdateUserAsync(
                    _mapper.Map<UpdateUserRequest>(input));
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Update user success"
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

        [HttpDelete("{id}")]
        public async Task<ResponseDto> DeleteUser(string id)
        {
            try
            {
                DeleteUserResponse response = await _userGrpcClient.DeleteUserAsync(
                    new DeleteUserRequest()
                    {
                        Id = id
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Delete user success"
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
