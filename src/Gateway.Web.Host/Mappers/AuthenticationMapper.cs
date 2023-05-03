using AutoMapper;
using Gateway.Core.Dtos.Authentications;
using Gateway.Web.Host.Protos;

namespace Gateway.Web.Host.Mappers
{
    public class AuthenticationMapper : Profile
    {
        public AuthenticationMapper() 
        {
            // Login
            CreateMap<LoginInputDto, LoginRequest>();

            // Register
            CreateMap<RegisterInputDto, RegisterRequest>();
        }

    }
}
