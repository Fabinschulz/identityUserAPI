using AutoMapper;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Application.Requests;
using IdentityUser.src.Domain.Entities;

namespace IdentityUser.src.Application.Mapper
{
    public sealed class CreateUserMapper : Profile
    {
        public CreateUserMapper()
        {
            CreateMap<CreateUserCommand, User>();
            CreateMap<User, CreateUserQuery>();
        }
    }
}
