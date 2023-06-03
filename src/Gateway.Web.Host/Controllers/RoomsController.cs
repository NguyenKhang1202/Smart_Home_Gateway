using AutoMapper;
using Gateway.Core.Dtos;
using Gateway.Core.Dtos.Rooms;
using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Rooms;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Web.Host.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RoomGrpc.RoomGrpcClient _roomGrpcClient;
        private readonly IAppSession _appSession;
        private readonly IMapper _mapper;
        public RoomsController(
            RoomGrpc.RoomGrpcClient roomGrpcClient,
            IAppSession appSession,
            IMapper mapper
            )
        {
            _roomGrpcClient = roomGrpcClient;
            _appSession = appSession;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ResponseDto> GetAllRooms([FromQuery] GetAllRoomsInputDto input)
        {
            try
            {
                GetAllRoomsResponse response = await _roomGrpcClient.GetAllRoomsAsync(
                    _mapper.Map<GetAllRoomsRequest>(input));
                return new ResponseDto()
                {
                    TotalCount = response.TotalCount,
                    Data = response.Items,
                    Success = true,
                    Message = "Get rooms success"
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
        public async Task<ResponseDto> GetRoomById(string id)
        {
            try
            {
                GetRoomByIdResponse response = await _roomGrpcClient.GetRoomByIdAsync(
                    new GetRoomByIdRequest()
                    {
                        Id = id,
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Get room success"
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

        [HttpPost("")]
        public async Task<ResponseDto> CreateRoom([FromBody] CreateRoomInputDto input)
        {
            try
            {
                CreateRoomRequest request = _mapper.Map<CreateRoomRequest>(input);
                request.UserId = _appSession.GetUserId();
                CreateRoomResponse response = await _roomGrpcClient.CreateRoomAsync(request);
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Create room success"
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
        public async Task<ResponseDto> UpdateRoom(string id, [FromBody] UpdateRoomInputDto input)
        {
            try
            {
                UpdateRoomResponse response = await _roomGrpcClient.UpdateRoomAsync(
                    _mapper.Map<UpdateRoomRequest>(input));
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Update room success"
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
        public async Task<ResponseDto> DeleteRoom(string id)
        {
            try
            {
                DeleteRoomResponse response = await _roomGrpcClient.DeleteRoomAsync(
                    new DeleteRoomRequest()
                    {
                        Id = id
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Delete room success"
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
