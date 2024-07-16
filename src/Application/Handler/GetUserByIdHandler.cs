using AutoMapper;
using FluentValidation;
using IdentityUser.src.Application.Common.Exceptions;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Application.Requests;
using IdentityUser.src.Domain.Interfaces;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand, GetUserByIdQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetUserByIdCommand> _validator;

        public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper, IValidator<GetUserByIdCommand> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetUserByIdQuery> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request);
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetById(request.Id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            return _mapper.Map<GetUserByIdQuery>(user);
        }
    }
}
