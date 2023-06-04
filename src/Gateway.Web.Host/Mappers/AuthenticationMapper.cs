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

            // Refresh token 
            CreateMap<RefreshTokenInputDto, RefreshTokenRequest>()
                .ForMember(d => d.RefreshToken, s => s.MapFrom(i => i.RefreshToken ?? ""));

            // Change password
            CreateMap<ChangePasswordInputDto, ChangePasswordRequest>()
                .ForMember(d => d.OldPassword, s => s.MapFrom(i => i.OldPassword ?? ""))
                .ForMember(d => d.NewPassword, s => s.MapFrom(i => i.NewPassword ?? ""));
        }

    }
}
