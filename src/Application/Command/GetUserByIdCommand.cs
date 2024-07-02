using IdentityUser.src.Application.Queries;
using MediatR;

namespace IdentityUser.src.Application.Requests
{
    public sealed record GetUserByIdCommand(Guid Id) : IRequest<GetUserByIdQuery>;
}
