using AutoMapper;
using FluentValidation;
using IdentityUser.src.Application.Command;
using IdentityUser.src.Application.Common.Exceptions;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Application.Queries.DTOs;
using IdentityUser.src.Domain.Interfaces.Repositories;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    /// <summary>
    /// Handles the request to get a user by their ID.
    /// </summary>
    public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand, GetUserByIdQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetUserByIdCommand> _validator;
        private readonly ILogger<GetUserByIdHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserByIdHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="validator">The validator for <see cref="GetUserByIdCommand"/>.</param>
        /// <param name="logger">The logger.</param>
        public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper, IValidator<GetUserByIdCommand> validator, ILogger<GetUserByIdHandler> logger)
        {
            _userRepository = userRepository;
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

            var user = await _userRepository.GetById(request.Id);
            if (user == null)
            {
                _logger.LogError("User not found for ID {Id}", request.Id);
                throw new NotFoundException("User not found");
            }

            _logger.LogInformation("----- Getting user by ID: {Id}", request.Id);
            return _mapper.Map<GetUserByIdQuery>(user);
        }
    }

    /// <summary>
    /// Provides mapping configurations for user-related operations.
    /// </summary>
    public sealed class GetUserByIdMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserByIdMapper"/> class.
        /// Configures the mappings between <see cref="UpdateUserCommand"/> and <see cref="UserDto"/>,
        /// and between <see cref="UserDto"/> and <see cref="GetUserByIdQuery"/>.
        /// </summary>        
        public GetUserByIdMapper()
        {
            CreateMap<UpdateUserCommand, UserDto>();
            CreateMap<UserDto, GetUserByIdQuery>();
        }
    }
}
