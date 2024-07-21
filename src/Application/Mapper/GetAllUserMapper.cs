using AutoMapper;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Application.Requests;
using IdentityUser.src.Domain.Entities;

namespace IdentityUser.src.Application.Mapper
{
    public sealed class GetAllUserMapper : Profile
    {
        public GetAllUserMapper()
        {
            CreateMap<GetAllUserCommand, User>();
            CreateMap<User, GetAllUserQuery>();
        }
    }
}
