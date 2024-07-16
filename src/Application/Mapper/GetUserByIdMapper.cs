using AutoMapper;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Application.Requests;
using IdentityUser.src.Domain.Entities;

namespace IdentityUser.src.Application.Mapper
{
    public sealed class GetUserByIdMapper : Profile
    {
        public GetUserByIdMapper()
        {
            CreateMap<UpdateUserCommand, User>();
            CreateMap<User, GetUserByIdQuery>();
        }
    }
}
