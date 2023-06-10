using AutoMapper;
using Gateway.Core.Dtos.Notifications;
using Gateway.Web.Host.Protos.Notifications;

namespace Gateway.Web.Host.Mappers
{
    public class NotificationMapper : Profile
    {
        public NotificationMapper()
        {
            CreateMap<GetAllNotificationsInputDto, GetAllNotificationsRequest>();
            CreateMap<CreateNotificationInputDto, CreateNotificationRequest>();
            CreateMap<UpdateNotificationInputDto, UpdateNotificationRequest>();

            CreateMap<UpdateStatusNotificationInputDto, UpdateStatusNotificationRequest>();
        }
    }
}
