using AutoMapper;
using FluentValidation;
using IdentityUser.src.Application.Command;
using IdentityUser.src.Application.Common.Exceptions;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Services;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    /// <summary>
    /// Handles the request to get a user by their ID.
    /// </summary>
    public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand, GetUserByIdQuery>
    {
        private readonly UserServices _userService;
        private readonly IMapper _mapper;
        private readonly IValidator<GetUserByIdCommand> _validator;
        private readonly ILogger<GetUserByIdHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserByIdHandler"/> class.
        /// </summary>
        /// <param name="userServices">The user service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="validator">The validator for <see cref="GetUserByIdCommand"/>.</param>
        /// <param name="logger">The logger.</param>
        public GetUserByIdHandler(UserServices userServices, IMapper mapper, IValidator<GetUserByIdCommand> validator, ILogger<GetUserByIdHandler> logger)
        {
            _userService = userServices;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        /// <summary>
        /// Handles the request to get a user by their ID.
        /// </summary>
        /// <param name="request">The request containing the user ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user details.</returns>
        /// <exception cref="ValidationException">Thrown when the request validation fails.</exception>
        /// <exception cref="NotFoundException">Thrown when the user is not found.</exception>
        public async Task<GetUserByIdQuery> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request);
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation failed for {Request}", request);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(request.Id);
                return _mapper.Map<GetUserByIdQuery>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling {Request}", request);
                throw new Exception("Error: " + ex.Message);
            }
        }
    }

    /// <summary>
    /// Provides mapping configurations for user-related operations.
    /// </summary>
    public sealed class GetUserByIdMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserByIdMapper"/> class.
        /// </summary>        
        public GetUserByIdMapper()
        {
            CreateMap<User, GetUserByIdQuery>();
        }
    }
}
