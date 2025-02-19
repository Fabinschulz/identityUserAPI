using AutoMapper;
using FluentValidation;
using IdentityUser.src.Application.Command;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Services;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    /// <summary>
    /// Handles the creation of a new user.
    /// </summary>
    public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserQuery>
    {
        private readonly IMapper _mapper;
        private readonly UserServices _userService;
        private readonly IValidator<CreateUserCommand> _validator;
        private readonly ILogger<CreateUserHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserHandler"/> class.
        /// </summary>
        /// <param name="mapper">The mapper to map between objects.</param>
        /// <param name="userServices">The user services to interact with the data store.</param>
        /// <param name="validator">The validator to validate the create user command.</param>
        /// <param name="logger">The logger.</param>
        public CreateUserHandler(IMapper mapper, UserServices userServices, IValidator<CreateUserCommand> validator, ILogger<CreateUserHandler> logger)
        {
            _mapper = mapper;
            _userService = userServices;
            _validator = validator;
            _logger = logger;
        }

        /// <summary>
        /// Handles the create user command.
        /// </summary>
        /// <param name="request">The create user command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the create user query.</returns>
        public async Task<CreateUserQuery> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            try
            {
                var mappedUser = _mapper.Map<User>(request);
                var registered = await _userService.CreateUserAsync(mappedUser);
                return _mapper.Map<CreateUserQuery>(registered);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in Command {CommandName} - {Error}", request.GetType().Name, e.Message);
                throw new Exception("Error: " + e.Message);
            }
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
