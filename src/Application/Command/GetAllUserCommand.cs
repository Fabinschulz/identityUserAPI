using IdentityUser.src.Application.Queries;
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
        string? Role) : IRequest<GetAllUserQuery>;

}
