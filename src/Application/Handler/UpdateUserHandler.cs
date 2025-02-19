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
    /// Handles the update of a user.
    /// </summary>
    public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserQuery>
    {
        private readonly UserServices _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserHandler> _logger;
        private readonly IValidator<UpdateUserCommand> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserHandler"/> class.
        /// </summary>
        /// <param name="userServices">The user services.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="validator">The validator.</param>
        public UpdateUserHandler(UserServices userServices, IMapper mapper, ILogger<UpdateUserHandler> logger, IValidator<UpdateUserCommand> validator)
        {
            _userService = userServices;
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
            await _validator.ValidateAndThrowAsync(request);

            try
            {
                var user = await _userService.UpdateUserAsync(request.user);
                return _mapper.Map<UpdateUserQuery>(user);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling {Request}", request);
                throw new Exception("Error: " + ex.Message);
            }
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
