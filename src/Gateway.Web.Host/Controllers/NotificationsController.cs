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
        public async Task<ResponseDto> GetAllNotifications([FromQuery] GetAllNotificationsInputDto input)
        {
            try
            {
                GetAllNotificationsRequest request = _mapper.Map<GetAllNotificationsRequest>(input);
                request.UserId = _appSession.GetUserId();
                GetAllNotificationsResponse response = await _notificationGrpcClient.GetAllNotificationsAsync(request);
                return new ResponseDto()
                {
                    TotalCount = response.TotalCount,
                    Data = response.Items,
                    Success = true,
                    Message = "Get notifications success"
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
        public async Task<ResponseDto> GetNotificationById(string id)
        {
            try
            {
                GetNotificationByIdResponse response = await _notificationGrpcClient.GetNotificationByIdAsync(
                    new GetNotificationByIdRequest()
                    {
                        Id = id,
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Get notification success"
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
        public async Task<ResponseDto> CreateNotification([FromBody] CreateNotificationInputDto input)
        {
            try
            {
                CreateNotificationRequest request = _mapper.Map<CreateNotificationRequest>(input);
                request.UserId = _appSession.GetUserId();
                CreateNotificationResponse response = await _notificationGrpcClient.CreateNotificationAsync(request);
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Create notification success"
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
        public async Task<ResponseDto> UpdateNotification(string id, [FromBody] UpdateNotificationInputDto input)
        {
            try
            {
                UpdateNotificationResponse response = await _notificationGrpcClient.UpdateNotificationAsync(
                    _mapper.Map<UpdateNotificationRequest>(input));
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Update notification success"
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

        [HttpPut("status")]
        public async Task<ResponseDto> UpdateStatusNotification(string id, [FromBody] UpdateStatusNotificationInputDto input)
        {
            try
            {
                UpdateStatusNotificationResponse response = await _notificationGrpcClient.UpdateStatusNotificationAsync(
                    _mapper.Map<UpdateStatusNotificationRequest>(input));
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Update status notification success"
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
        public async Task<ResponseDto> DeleteNotification(string id)
        {
            try
            {
                DeleteNotificationResponse response = await _notificationGrpcClient.DeleteNotificationAsync(
                    new DeleteNotificationRequest()
                    {
                        Id = id
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Delete notification success"
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
