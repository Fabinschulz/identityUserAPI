using IdentityUser.src.Application.Queries;
using MediatR;

namespace IdentityUser.src.Application.Requests
{
    public sealed record UpdateUserCommand(Guid id, string Username, string Email, string Role, bool IsDeleted) : IRequest<UpdateUserQuery>;
}
