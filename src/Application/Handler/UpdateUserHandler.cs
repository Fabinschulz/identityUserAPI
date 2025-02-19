using AutoMapper;
using FluentValidation;
using IdentityUser.src.Application.Command;
using IdentityUser.src.Application.Common.Exceptions;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Interfaces.Repositories;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    /// <summary>
    /// Handles the update of a user.
    /// </summary>
    public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserHandler> _logger;
        private readonly IValidator<UpdateUserCommand> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="validator">The validator.</param>
        public UpdateUserHandler(IUserRepository userRepository, IMapper mapper, ILogger<UpdateUserHandler> logger, IValidator<UpdateUserCommand> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        /// <summary>
        /// Handles the update user command.
        /// </summary>
        /// <param name="request">The update user command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated user query.</returns>
        public async Task<UpdateUserQuery> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);

            var user = await _userRepository.GetById(request.Id);
            EnsureUserExists(user, request.Id);

            UpdateUserProperties(user, request);
            await _userRepository.Update(user);

            var userResponse = MapToUserResponse(user);

            _logger.LogInformation("----- Command result: {@Result} - User updated: {Id} ({@Command})", userResponse, request.Id, request);
            return userResponse;
        }

        /// <summary>
        /// Validates the update user command.
        /// </summary>
        /// <param name="request">The update user command.</param>
        private async Task ValidateRequest(UpdateUserCommand request)
        {
            await _validator.ValidateAndThrowAsync(request);
        }

        /// <summary>
        /// Ensures the user exists.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userId">The user identifier.</param>
        /// <exception cref="NotFoundException">Thrown when the user is not found.</exception>
        private void EnsureUserExists(User user, Guid userId)
        {
            if (user == null)
            {
                string errorMessage = $"Usuário com id: {userId} não foi encontrado no banco de dados.";
                _logger.LogError("----- User not found: {Id}", userId);
                throw new NotFoundException(errorMessage);
            }
        }

        /// <summary>
        /// Updates the user properties.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="request">The update user command.</param>
        private void UpdateUserProperties(User user, UpdateUserCommand request)
        {
            user.Username = request.Username;
            user.Email = request.Email;
            user.Role = request.Role;
            user.IsDeleted = request.IsDeleted;
        }

        /// <summary>
        /// Maps the user to the user response.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The updated user query.</returns>
        private UpdateUserQuery MapToUserResponse(User user)
        {
            return _mapper.Map<UpdateUserQuery>(user);
        }
    }

    /// <summary>
    /// Provides mapping configurations for updating a user.
    /// </summary>
    public sealed class UpdateUserMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserMapper"/> class.
        /// Configures the mappings between <see cref="UpdateUserCommand"/> and <see cref="User"/>,
        /// and between <see cref="User"/> and <see cref="UpdateUserQuery"/>.
        /// </summary>
        public UpdateUserMapper()
        {
            CreateMap<UpdateUserCommand, User>();
            CreateMap<User, UpdateUserQuery>();
        }
    }
}
