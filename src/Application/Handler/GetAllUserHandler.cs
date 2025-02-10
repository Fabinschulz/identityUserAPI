using AutoMapper;
using FluentValidation;
using IdentityUser.src.Application.Command;
using IdentityUser.src.Application.Common.Models;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Interfaces;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    /// <summary>
    /// Handles the request to get all users.
    /// </summary>
    public sealed class GetAllUserHandler : IRequestHandler<GetAllUserCommand, GetAllUserQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetAllUserCommand> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllUserHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="validator">The validator for the command.</param>
        public GetAllUserHandler(IUserRepository userRepository, IMapper mapper, IValidator<GetAllUserCommand> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
        }

        /// <summary>
        /// Handles the request to get all users.
        /// </summary>
        /// <param name="request">The request command containing the parameters for retrieving users.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the query result with the list of users.</returns>
        /// <exception cref="ValidationException">Thrown when the request command validation fails.</exception>
        public async Task<GetAllUserQuery> Handle(GetAllUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
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

            var usersMapped = _mapper.Map<ListDataPagination<User>>(users);
            return _mapper.Map<GetAllUserQuery>(usersMapped);
        }
    }

    /// <summary>
    /// Provides mapping configurations for user-related commands and queries.
    /// </summary>
    public sealed class GetAllUserMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllUserMapper"/> class.
        /// Configures the mappings between <see cref="GetAllUserCommand"/> and <see cref="User"/>,
        /// and between <see cref="User"/> and <see cref="GetAllUserQuery"/>.
        /// </summary>
        public GetAllUserMapper()
        {
            CreateMap<GetAllUserCommand, User>();
            CreateMap<User, GetAllUserQuery>();
        }
    }
}
