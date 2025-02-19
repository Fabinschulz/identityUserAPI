using IdentityUser.src.Application.Command;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Services;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    /// <summary>
    /// Handles the deletion of a user.
    /// </summary>
    public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserByIdQuery>
    {
        private readonly UserServices _userService;
        private readonly ILogger<DeleteUserHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserHandler"/> class.
        /// </summary>
        /// <param name="userServices">The user services.</param>
        /// <param name="logger">The logger.</param>
        public DeleteUserHandler(UserServices userServices, ILogger<DeleteUserHandler> logger)
        {
            _userService = userServices;
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
            try
            {
                var isDeleted = await _userService.DeleteUserAsync(request.Id);
                var message = isDeleted ? "Usuário deletado com sucesso." : "Falha ao deletar o usuário.";
                return new DeleteUserByIdQuery(isDeleted, message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in Command {CommandName} - {Error}", request.GetType().Name, e.Message);
                throw new Exception("Error: " + e.Message);
            }
        }
    }
}
