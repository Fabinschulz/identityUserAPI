using IdentityUser.src.Application.Queries;
using MediatR;

namespace IdentityUser.src.Application.Requests
{
    public sealed record CreateUserCommand(string Email, string Password) : IRequest<CreateUserQuery>;

}
