using IdentityUser.src.Application.Queries;
using MediatR;

namespace IdentityUser.src.Application.Command
{
    /// <summary>
    /// Command to delete a user by their unique identifier.
    /// </summary>
    /// <param name="Id">The unique identifier of the user to be deleted.</param>
    public sealed record DeleteUserCommand(Guid Id) : IRequest<DeleteUserByIdQuery>;
}
