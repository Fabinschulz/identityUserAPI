using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Enums;
using MediatR;

namespace IdentityUser.src.Application.Requests
{
    public sealed record GetAllUserCommand(
        int Page,
        int Size,
        string? Username,
        string? Email,
        bool IsDeleted,
        string? OrderBy,
        RoleEnum? Role)
        : IRequest<GetAllUserQuery>;
}
