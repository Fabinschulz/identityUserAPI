using FluentValidation;
using IdentityUser.src.Application.Command;
using IdentityUser.src.Application.Common.Models;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Interfaces;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    /// <summary>
    /// Handles the request to get all users.
    /// </summary>
    public sealed class GetAllUserHandler : IRequestHandler<GetAllUserCommand, ListDataPagination<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<GetAllUserCommand> _validator;
        private readonly ILogger<GetAllUserHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllUserHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="validator">The validator for the command.</param>
        /// <param name="logger">The logger.</param>
        public GetAllUserHandler(IUserRepository userRepository, IValidator<GetAllUserCommand> validator, ILogger<GetAllUserHandler> logger)
        {
            _userRepository = userRepository;
            _validator = validator;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handles the request to get all users.
        /// </summary>
        /// <param name="request">The request command containing the parameters for retrieving users.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the query result with the list of users.</returns>
        /// <exception cref="ValidationException">Thrown when the request command validation fails.</exception>
        public async Task<ListDataPagination<User>> Handle(GetAllUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation failed for {Request}", request);
                throw new ValidationException(validationResult.Errors);
            }

            var users = await _userRepository.GetAll(
                request.Page,
                request.Size,
                request.Username,
                request.Email,
                request.IsDeleted,
                request.OrderBy,
                request.Role
                );

            _logger.LogInformation("Handling {Request}", request);
            return users;
        }
    }
}
