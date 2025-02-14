using IdentityUser.src.Application.Command;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Interfaces;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    /// <summary>
    /// Handles the deletion of a user.
    /// </summary>
    public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserByIdQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeleteUserHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="logger">The logger.</param>
        public DeleteUserHandler(IUserRepository userRepository, ILogger<DeleteUserHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Handles the delete user command.
        /// </summary>
        /// <param name="request">The delete user command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the delete user by ID query.</returns>
        public async Task<DeleteUserByIdQuery> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await DeleteUserInRepository(request.Id);

            var message = isDeleted ? "Usuário deletado com sucesso." : "Falha ao deletar o usuário.";
            _logger.LogInformation("----- Command result: {@Result} - {CommandName}: {CommandId} ({@Command})", message, nameof(DeleteUserCommand.Id), request.Id, request);
            return new DeleteUserByIdQuery(isDeleted, message);
        }

        /// <summary>
        /// Deletes the user in the repository.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the user was deleted.</returns>
        private async Task<bool> DeleteUserInRepository(Guid id)
        {
            return await _userRepository.Delete(id);
        }
    }
}
