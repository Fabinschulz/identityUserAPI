using IdentityUser.src.Application.Queries;
using IdentityUser.src.Application.Requests;
using IdentityUser.src.Domain.Interfaces;
using MediatR;


namespace IdentityUser.src.Application.Handler
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserByIdQuery>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<DeleteUserByIdQuery> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await DeleteUserInRepository(request.Id);

            var message = isDeleted ? "Usuário deletado com sucesso." : "Falha ao deletar o usuário.";
            return new DeleteUserByIdQuery(isDeleted, message);
        }

        private async Task<bool> DeleteUserInRepository(Guid id)
        {
            return await _userRepository.Delete(id);
        }
    }
}
