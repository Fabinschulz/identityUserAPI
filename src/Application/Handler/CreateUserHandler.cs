using IdentityUser.src.Application.Queries;
using IdentityUser.src.Application.Requests;
using MediatR;
using FluentValidation;
using AutoMapper;
using IdentityUser.src.Domain.Interfaces;
using IdentityUser.src.Domain.Entities;

namespace IdentityUser.src.Application.Handler
{
    public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserQuery>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUserCommand> _validator;

        public CreateUserHandler(IMapper mapper, IUserRepository userRepository, IValidator<CreateUserCommand> validator)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<CreateUserQuery> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            await ValidateRequest(request, cancellationToken);

            var mappedUser = mapUser(request);
            var registered = await Register(mappedUser);
            var response = MapUserToResponse(registered);

            return response;
        }

        private async Task ValidateRequest(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
        }

        private User mapUser(CreateUserCommand request)
        {
            return _mapper.Map<User>(request);

        }

        private async Task<User> Register(User user)
        {
            return await _userRepository.Register(user);
        }

        private CreateUserQuery MapUserToResponse(User user)
        {
            return _mapper.Map<CreateUserQuery>(user);
        }


    }
}
