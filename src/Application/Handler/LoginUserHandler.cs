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
    /// Handles the login process for a user.
    /// </summary>
    public sealed class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserQuery>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<LoginUserCommand> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginUserHandler"/> class.
        /// </summary>
        /// <param name="mapper">The mapper to map user entities to queries.</param>
        /// <param name="userRepository">The repository to access user data.</param>
        /// <param name="validator">The validator to validate login commands.</param>
        public LoginUserHandler(IMapper mapper, IUserRepository userRepository, IValidator<LoginUserCommand> validator)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _validator = validator;
        }

        /// <summary>
        /// Handles the login request.
        /// </summary>
        /// <param name="request">The login command containing user credentials.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user query.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user credentials are invalid.</exception>
        public async Task<LoginUserQuery> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request);

            var user = await _userRepository.Login(request.Email, request.Password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }

            var userQuery = _mapper.Map<LoginUserQuery>(user);
            return userQuery;
        }
    }

    /// <summary>
    /// Maps properties from the <see cref="User"/> entity to the <see cref="LoginUserQuery"/> DTO.
    /// </summary>
    public sealed class LoginUserMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginUserMapper"/> class.
        /// Configures the mapping between <see cref="User"/> and <see cref="LoginUserQuery"/>.
        /// </summary>
        public LoginUserMapper()
        {
            CreateMap<User, LoginUserQuery>()
              .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token));
        }
    }
}
