using AutoMapper;
using FluentValidation;
using IdentityUser.src.Application.Command;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Interfaces;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    /// <summary>
    /// Handles the creation of a new user.
    /// </summary>
    public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserQuery>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUserCommand> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserHandler"/> class.
        /// </summary>
        /// <param name="mapper">The mapper to map between objects.</param>
        /// <param name="userRepository">The user repository to interact with the data store.</param>
        /// <param name="validator">The validator to validate the create user command.</param>
        public CreateUserHandler(IMapper mapper, IUserRepository userRepository, IValidator<CreateUserCommand> validator)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _validator = validator;
        }

        /// <summary>
        /// Handles the create user command.
        /// </summary>
        /// <param name="request">The create user command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the create user query.</returns>
        public async Task<CreateUserQuery> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request, cancellationToken);

            var mappedUser = mapUser(request);
            var registered = await Register(mappedUser);
            var response = MapUserToResponse(registered);

            return response;
        }

        /// <summary>
        /// Validates the create user command.
        /// </summary>
        /// <param name="request">The create user command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task ValidateRequest(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
        }

        /// <summary>
        /// Maps the create user command to a user entity.
        /// </summary>
        /// <param name="request">The create user command.</param>
        /// <returns>The mapped user entity.</returns>
        private User mapUser(CreateUserCommand request)
        {
            return _mapper.Map<User>(request);
        }

        /// <summary>
        /// Registers the user in the data store.
        /// </summary>
        /// <param name="user">The user entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the registered user entity.</returns>
        private async Task<User> Register(User user)
        {
            return await _userRepository.Register(user);
        }

        /// <summary>
        /// Maps the user entity to a create user query.
        /// </summary>
        /// <param name="user">The user entity.</param>
        /// <returns>The mapped create user query.</returns>
        private CreateUserQuery MapUserToResponse(User user)
        {
            return _mapper.Map<CreateUserQuery>(user);
        }
    }

    /// <summary>
    /// Represents a mapper profile for creating a user.
    /// </summary>
    public sealed class CreateUserMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserMapper"/> class.
        /// Configures the mappings between <see cref="CreateUserCommand"/> and <see cref="User"/>,
        /// and between <see cref="User"/> and <see cref="CreateUserQuery"/>.
        /// </summary>
        public CreateUserMapper()
        {
            CreateMap<CreateUserCommand, User>();
            CreateMap<User, CreateUserQuery>();
        }
    }
}
