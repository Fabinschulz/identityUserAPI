using AutoMapper;
using FluentValidation;
using IdentityUser.src.Application.Common.Exceptions;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Application.Requests;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Interfaces;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserHandler> _logger;
        private readonly IValidator<UpdateUserCommand> _validator;

        public UpdateUserHandler(IUserRepository userRepository, IMapper mapper, ILogger<UpdateUserHandler> logger, IValidator<UpdateUserCommand> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<UpdateUserQuery> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);

            var user = await _userRepository.GetById(request.Id);
            EnsureUserExists(user, request.Id);

            UpdateUserProperties(user, request);
            await _userRepository.Update(user);

            var userResponse = MapToUserResponse(user);
            return userResponse;
        }

        private async Task ValidateRequest(UpdateUserCommand request)
        {
            await _validator.ValidateAndThrowAsync(request);
        }

        private void EnsureUserExists(User user, Guid userId)
        {
            if (user == null)
            {
                string errorMessage = $"Usuário com id: {userId} não foi encontrado no banco de dados.";
                _logger.LogError(errorMessage);
                throw new NotFoundException(errorMessage);
            }
        }

        private void UpdateUserProperties(User user, UpdateUserCommand request)
        {
            user.Username = request.Username;
            user.Email = request.Email;
            user.Role = request.Role;
            user.IsDeleted = request.IsDeleted;
        }

        private UpdateUserQuery MapToUserResponse(User user)
        {
            return _mapper.Map<UpdateUserQuery>(user);
        }

    }
}
