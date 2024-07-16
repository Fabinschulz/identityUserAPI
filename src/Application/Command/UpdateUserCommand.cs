using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Enums;
using MediatR;
using System.Text.Json.Serialization;

namespace IdentityUser.src.Application.Requests
{
    public sealed record UpdateUserCommand(
        Guid Id,
        string Username,
        string Email,
        [property: JsonConverter(typeof(RoleEnumConverter))] RoleEnum Role,
        bool IsDeleted
    ) : IRequest<UpdateUserQuery>;
}
