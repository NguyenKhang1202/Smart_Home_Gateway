using AutoMapper;
using Gateway.Core.Dtos.Users;
using Gateway.Web.Host.Protos.Users;

namespace Gateway.Web.Host.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper() 
        {
            CreateMap<GetAllUsersInputDto, GetAllUsersRequest>();
            CreateMap<UpdateUserInputDto, UpdateUserRequest>()
                .ForMember(d => d.Email, s => s.MapFrom(i => i.Email ?? ""))
                .ForMember(d => d.PhoneNumber, s => s.MapFrom(i => i.PhoneNumber ?? ""))
                .ForMember(d => d.Firstname, s => s.MapFrom(i => i.Firstname ?? ""))
                .ForMember(d => d.Lastname, s => s.MapFrom(i => i.Lastname ?? ""));
        }
    }
}
