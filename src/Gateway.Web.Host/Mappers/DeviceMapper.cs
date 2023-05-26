using AutoMapper;
using Gateway.Core.Dtos.Devices;
using Gateway.Web.Host.Protos.Devices;

namespace Gateway.Web.Host.Mappers
{
    public class DeviceMapper : Profile
    {
        public DeviceMapper() 
        {
            CreateMap<GetAllDevicesInputDto, GetAllDevicesRequest>()
                .ForMember(d => d.HomeDeviceId, s => s.MapFrom(i => i.HomeDeviceId ?? ""))
                .ForMember(d => d.RoomId, s => s.MapFrom(i => i.RoomId ?? ""));
            CreateMap<CreateDeviceInputDto, CreateDeviceRequest>()
               .ForMember(d => d.TenantId, s => s.MapFrom(i => i.TenantId ?? ""))
               .ForMember(d => d.UserId, s => s.MapFrom(i => i.UserId ?? ""))
               .ForMember(d => d.DeviceCode, s => s.MapFrom(i => i.DeviceCode ?? ""))
               .ForMember(d => d.Name, s => s.MapFrom(i => i.Name ?? ""))
               .ForMember(d => d.Properties, s => s.MapFrom(i => i.Properties ?? ""))
               .ForMember(d => d.HomeDeviceId, s => s.MapFrom(i => i.HomeDeviceId ?? ""))
               .ForMember(d => d.RoomId, s => s.MapFrom(i => i.RoomId ?? ""))
               .ForMember(d => d.Producer, s => s.MapFrom(i => i.Producer ?? ""))
               .ForMember(d => d.ImagesUrl, s => s.MapFrom(i => i.ImagesUrl ?? new List<string>()));
            CreateMap<UpdateDeviceInputDto, UpdateDeviceRequest>()
                .ForMember(d => d.Id, s => s.MapFrom(i => i.Id ?? ""))
                .ForMember(d => d.DeviceCode, s => s.MapFrom(i => i.DeviceCode ?? ""))
                .ForMember(d => d.Name, s => s.MapFrom(i => i.Name ?? ""))
                .ForMember(d => d.Properties, s => s.MapFrom(i => i.Properties ?? ""))
                .ForMember(d => d.ImagesUrl, s => s.MapFrom(i => i.ImagesUrl ?? new List<string>()));
        }
    }
}
