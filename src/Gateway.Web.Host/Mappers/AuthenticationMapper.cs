using AutoMapper;
using Gateway.Core.Dtos.Authentications;
using Gateway.Web.Host.Protos;
using Gateway.Web.Host.Protos.Authentications;

namespace Gateway.Web.Host.Mappers
{
    public class AuthenticationMapper : Profile
    {
        public AuthenticationMapper() 
        {
            // Login
            CreateMap<LoginInputDto, LoginRequest>();

            // Register
            CreateMap<RegisterInputDto, RegisterRequest>()
                .ForMember(d => d.PhoneNumber, s => s.MapFrom(i => i.PhoneNumber ?? ""));
        }

    }
}
