using AutoMapper;
using Gateway.Core.Dtos;
using Gateway.Core.Dtos.Notifications;
using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Notifications;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Web.Host.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationGrpc.NotificationGrpcClient _notificationGrpcClient;
        private readonly IAppSession _appSession;
        private readonly IMapper _mapper;
        public NotificationsController(
            NotificationGrpc.NotificationGrpcClient notificationGrpcClient,
            IAppSession appSession,
            IMapper mapper
            )
        {
            _notificationGrpcClient = notificationGrpcClient;
            _appSession = appSession;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllNotifications([FromQuery] GetAllNotificationsInputDto input)
        {
            try
            {
                GetAllNotificationsRequest request = _mapper.Map<GetAllNotificationsRequest>(input);
                request.UserId = _appSession.GetUserId();
                GetAllNotificationsResponse response = await _notificationGrpcClient.GetAllNotificationsAsync(request);
                return Ok(new ResponseDto()
                {
                    TotalCount = response.TotalCount,
                    Data = response.Items,
                    Success = true,
                    Message = "Get notifications success"
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
                    Message = "Get notifications failed"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(string id)
        {
            try
            {
                GetNotificationByIdResponse response = await _notificationGrpcClient.GetNotificationByIdAsync(
                    new GetNotificationByIdRequest()
                    {
                        Id = id,
                    });
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Get notification success"
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
                    Message = "Get notification failed"
                });
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationInputDto input)
        {
            try
            {
                CreateNotificationRequest request = _mapper.Map<CreateNotificationRequest>(input);
                request.UserId = _appSession.GetUserId();
                CreateNotificationResponse response = await _notificationGrpcClient.CreateNotificationAsync(request);
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Create notification success"
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
                    Message = "Create notification failed"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(string id, [FromBody] UpdateNotificationInputDto input)
        {
            try
            {
                UpdateNotificationResponse response = await _notificationGrpcClient.UpdateNotificationAsync(
                    _mapper.Map<UpdateNotificationRequest>(input));
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Update notification success"
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
                    Message = "Update notification failed"
                });
            }
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatusNotification(string id, [FromBody] UpdateStatusNotificationInputDto input)
        {
            try
            {
                UpdateStatusNotificationResponse response = await _notificationGrpcClient.UpdateStatusNotificationAsync(
                    _mapper.Map<UpdateStatusNotificationRequest>(input));
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Update status notification success"
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
                    Message = "Update status notification failed"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(string id)
        {
            try
            {
                DeleteNotificationResponse response = await _notificationGrpcClient.DeleteNotificationAsync(
                    new DeleteNotificationRequest()
                    {
                        Id = id
                    });
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Delete notification success"
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
                    Message = "Delete notification failed"
                });
            }
        }
    }
}
