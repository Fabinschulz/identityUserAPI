using AutoMapper;
using FluentValidation;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Application.Requests;
using IdentityUser.src.Domain.Interfaces;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    public sealed class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserQuery>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<LoginUserCommand> _validator;

        public LoginUserHandler(IMapper mapper, IUserRepository userRepository, IValidator<LoginUserCommand> validator)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _validator = validator;
        }

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
}
