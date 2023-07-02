using AutoMapper;
using Gateway.Core.Dtos;
using Gateway.Core.Dtos.Rooms;
using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Rooms;
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
        public async Task<IActionResult> GetAllRooms([FromQuery] GetAllRoomsInputDto input)
        {
            try
            {
                GetAllRoomsResponse response = await _roomGrpcClient.GetAllRoomsAsync(
                    _mapper.Map<GetAllRoomsRequest>(input));
                return Ok(new ResponseDto()
                {
                    TotalCount = response.TotalCount,
                    Data = response.Items,
                    Success = true,
                    Message = "Get rooms success"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Get rooms failed"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(string id)
        {
            try
            {
                GetRoomByIdResponse response = await _roomGrpcClient.GetRoomByIdAsync(
                    new GetRoomByIdRequest()
                    {
                        Id = id,
                    });
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Get room success"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Get room failed"
                });
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomInputDto input)
        {
            try
            {
                CreateRoomRequest request = _mapper.Map<CreateRoomRequest>(input);
                request.UserId = _appSession.GetUserId();
                CreateRoomResponse response = await _roomGrpcClient.CreateRoomAsync(request);
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Create room success"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Create room failed"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(string id, [FromBody] UpdateRoomInputDto input)
        {
            try
            {
                UpdateRoomResponse response = await _roomGrpcClient.UpdateRoomAsync(
                    _mapper.Map<UpdateRoomRequest>(input));
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Update room success"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Update room failed"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            try
            {
                DeleteRoomResponse response = await _roomGrpcClient.DeleteRoomAsync(
                    new DeleteRoomRequest()
                    {
                        Id = id
                    });
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Delete room success"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Delete room failed"
                });
            }
        }
    }
}
