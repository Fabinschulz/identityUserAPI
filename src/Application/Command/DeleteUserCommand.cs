using IdentityUser.src.Application.Queries;
using MediatR;

namespace IdentityUser.src.Application.Requests
{
    public abstract class DeleteUserCommand: IRequest<DeleteUserByIdQuery>
    {
        public Guid Id { get; }

        protected DeleteUserCommand(Guid id)
        {
            Id = id;
        }
    }
}
