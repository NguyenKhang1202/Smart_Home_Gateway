using AutoMapper;
using Gateway.Core.Dtos.Rooms;
using Gateway.Web.Host.Protos.Rooms;

namespace Gateway.Web.Host.Mappers
{
    public class RoomMapper : Profile
    {
        public RoomMapper() 
        {
            CreateMap<GetAllRoomsInputDto, GetAllRoomsRequest>();
            CreateMap<CreateRoomInputDto, CreateRoomRequest>()
               .ForMember(d => d.TenantId, s => s.MapFrom(i => i.TenantId ?? ""))
               .ForMember(d => d.UserId, s => s.MapFrom(i => i.UserId ?? ""))
               .ForMember(d => d.RoomCode, s => s.MapFrom(i => i.RoomCode ?? ""))
               .ForMember(d => d.Name, s => s.MapFrom(i => i.Name ?? ""))
               .ForMember(d => d.Properties, s => s.MapFrom(i => i.Properties ?? ""))
               .ForMember(d => d.HomeId, s => s.MapFrom(i => i.HomeId ?? ""))
               .ForMember(d => d.ImagesUrl, s => s.MapFrom(i => i.ImagesUrl ?? new List<string>()));
            CreateMap<UpdateRoomInputDto, UpdateRoomRequest>()
                .ForMember(d => d.Id, s => s.MapFrom(i => i.Id ?? ""))
                .ForMember(d => d.RoomCode, s => s.MapFrom(i => i.RoomCode ?? ""))
                .ForMember(d => d.Name, s => s.MapFrom(i => i.Name ?? ""))
                .ForMember(d => d.Properties, s => s.MapFrom(i => i.Properties ?? ""))
                .ForMember(d => d.ImagesUrl, s => s.MapFrom(i => i.ImagesUrl ?? new List<string>()));
        }
    }
}
