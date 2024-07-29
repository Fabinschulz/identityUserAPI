using AutoMapper;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Entities;

namespace IdentityUser.src.Application.Mapper
{
    public sealed class LoginUserMapper : Profile
    {
        public LoginUserMapper()
        {
            CreateMap<User, LoginUserQuery>()
              .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token));
        }
    }
}
