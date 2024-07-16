using AutoMapper;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Application.Requests;
using IdentityUser.src.Domain.Entities;

namespace IdentityUser.src.Application.Mapper
{
    public sealed class UpdateUserMapper : Profile
    {
        public UpdateUserMapper()
        {
            CreateMap<UpdateUserCommand, User>();
            CreateMap<User, UpdateUserQuery>();
        }
    }
}
