using IdentityUser.src.Application.Queries;
using MediatR;

namespace IdentityUser.src.Application.Requests
{
    public sealed record LoginUserCommand(string Email, string Password) : IRequest<LoginUserQuery>;

}
