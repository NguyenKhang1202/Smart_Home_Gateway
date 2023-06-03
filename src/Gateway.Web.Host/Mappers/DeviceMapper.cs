using AutoMapper;
using Gateway.Core.Dtos.Devices;
using Gateway.Core.Dtos.Gateways;
using Gateway.Core.Entities;
using Gateway.Web.Host.Protos.Devices;

namespace Gateway.Web.Host.Mappers
{
    public class DeviceMapper : Profile
    {
        public DeviceMapper() 
        {
            // Device
            CreateMap<GetAllDevicesInputDto, GetAllDevicesRequest>()
                .ForMember(d => d.HomeDeviceId, s => s.MapFrom(i => i.HomeDeviceId ?? ""))
                .ForMember(d => d.RoomId, s => s.MapFrom(i => i.RoomId ?? ""));
            CreateMap<CreateDeviceInputDto, CreateDeviceRequest>()
               .ForMember(d => d.TenantId, s => s.MapFrom(i => i.TenantId ?? ""))
               .ForMember(d => d.DeviceCode, s => s.MapFrom(i => i.DeviceCode ?? ""))
               .ForMember(d => d.Name, s => s.MapFrom(i => i.Name ?? ""))
               .ForMember(d => d.Properties, s => s.MapFrom(i => i.Properties ?? ""))
               .ForMember(d => d.HomeDeviceId, s => s.MapFrom(i => i.HomeDeviceId ?? ""))
               .ForMember(d => d.GatewayCode, s => s.MapFrom(i => i.GatewayCode ?? ""))
               .ForMember(d => d.RoomId, s => s.MapFrom(i => i.RoomId ?? ""))
               .ForMember(d => d.Producer, s => s.MapFrom(i => i.Producer ?? ""))
               .ForMember(d => d.ImagesUrl, s => s.MapFrom(i => i.ImagesUrl ?? new List<string>()));
            CreateMap<UpdateDeviceInputDto, UpdateDeviceRequest>()
                .ForMember(d => d.Id, s => s.MapFrom(i => i.Id ?? ""))
                .ForMember(d => d.DeviceCode, s => s.MapFrom(i => i.DeviceCode ?? ""))
                .ForMember(d => d.Name, s => s.MapFrom(i => i.Name ?? ""))
                .ForMember(d => d.Properties, s => s.MapFrom(i => i.Properties ?? ""))
                .ForMember(d => d.GatewayCode, s => s.MapFrom(i => i.GatewayCode ?? ""))
                .ForMember(d => d.ImagesUrl, s => s.MapFrom(i => i.ImagesUrl ?? new List<string>()));

            CreateMap<ControlDeviceInputDto, ControlDeviceRequest>()
                .ForMember(d => d.Id, s => s.MapFrom(i => i.Id ?? ""))
                .ForMember(d => d.Control, s => s.MapFrom(i => new PControl()
                {
                    Status = i.Control.Status,
                    Mode = i.Control.Mode,
                    Direction = i.Control.Direction,
                    Speed = i.Control.Speed,
                    Intensity = i.Control.Intensity,
                }));
            CreateMap<PControl, ControlDevice>()
                .ForMember(d => d.Status, s => s.MapFrom(i => i.Status))
                .ForMember(d => d.Mode, s => s.MapFrom(i => i.Mode))
                .ForMember(d => d.Direction, s => s.MapFrom(i => i.Direction))
                .ForMember(d => d.Speed, s => s.MapFrom(i => i.Speed))
                .ForMember(d => d.Intensity, s => s.MapFrom(i => i.Intensity));

            // Gateway
            CreateMap<GetAllGatewaysInputDto, GetAllGatewaysRequest>()
                .ForMember(d => d.HomeId, s => s.MapFrom(i => i.HomeId ?? ""));
            CreateMap<CreateGatewayInputDto, CreateGatewayRequest>()
               .ForMember(d => d.TenantId, s => s.MapFrom(i => i.TenantId))
               .ForMember(d => d.Name, s => s.MapFrom(i => i.Name ?? ""))
               .ForMember(d => d.DeviceCode, s => s.MapFrom(i => i.DeviceCode ?? ""))
               .ForMember(d => d.UserName, s => s.MapFrom(i => i.UserName ?? ""))
               .ForMember(d => d.Password, s => s.MapFrom(i => i.Password ?? ""))
               .ForMember(d => d.HomeId, s => s.MapFrom(i => i.HomeId ?? ""))
               .ForMember(d => d.Key, s => s.MapFrom(i => i.Key ?? ""))
               .ForMember(d => d.Ip, s => s.MapFrom(i => i.Ip ?? ""))
               .ForMember(d => d.Port, s => s.MapFrom(i => i.Port));
            CreateMap<UpdateGatewayInputDto, UpdateGatewayRequest>()
               .ForMember(d => d.Id, s => s.MapFrom(i => i.Id ?? ""))
               .ForMember(d => d.TenantId, s => s.MapFrom(i => i.TenantId))
               .ForMember(d => d.Name, s => s.MapFrom(i => i.Name ?? ""))
               .ForMember(d => d.DeviceCode, s => s.MapFrom(i => i.DeviceCode ?? ""))
               .ForMember(d => d.UserName, s => s.MapFrom(i => i.UserName ?? ""))
               .ForMember(d => d.Password, s => s.MapFrom(i => i.Password ?? ""))
               .ForMember(d => d.HomeId, s => s.MapFrom(i => i.HomeId ?? ""))
               .ForMember(d => d.Key, s => s.MapFrom(i => i.Key ?? ""))
               .ForMember(d => d.Ip, s => s.MapFrom(i => i.Ip ?? ""))
               .ForMember(d => d.Port, s => s.MapFrom(i => i.Port));
        }
    }
}
