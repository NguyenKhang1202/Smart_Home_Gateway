using AutoMapper;
using Gateway.Core.Dtos.Homes;
using Gateway.Web.Host.Protos.Homes;

namespace Gateway.Web.Host.Mappers
{
    public class HomeMapper : Profile
    {
        public HomeMapper() 
        {
            CreateMap<GetAllHomesInputDto, GetAllHomesRequest>();
            CreateMap<CreateHomeInputDto, CreateHomeRequest>()
               .ForMember(d => d.TenantId, s => s.MapFrom(i => i.TenantId ?? ""))
               .ForMember(d => d.UserId, s => s.MapFrom(i => i.UserId ?? ""))
               .ForMember(d => d.SmartHomeCode, s => s.MapFrom(i => i.SmartHomeCode ?? ""))
               .ForMember(d => d.Name, s => s.MapFrom(i => i.Name ?? ""))
               .ForMember(d => d.Properties, s => s.MapFrom(i => i.Properties ?? ""))
               .ForMember(d => d.Address, s => s.MapFrom(i => i.Address ?? ""))
               .ForMember(d => d.ImagesUrl, s => s.MapFrom(i => i.ImagesUrl ?? new List<string>()));
            CreateMap<UpdateHomeInputDto, UpdateHomeRequest>()
                .ForMember(d => d.Id, s => s.MapFrom(i => i.Id ?? ""))
                .ForMember(d => d.SmartHomeCode, s => s.MapFrom(i => i.SmartHomeCode ?? ""))
                .ForMember(d => d.Name, s => s.MapFrom(i => i.Name ?? ""))
                .ForMember(d => d.Properties, s => s.MapFrom(i => i.Properties ?? ""))
                .ForMember(d => d.Address, s => s.MapFrom(i => i.Address ?? ""))
                .ForMember(d => d.ImagesUrl, s => s.MapFrom(i => i.ImagesUrl ?? new List<string>()));
        }
    }
}
