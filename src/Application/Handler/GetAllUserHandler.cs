using AutoMapper;
using FluentValidation;
using IdentityUser.src.Application.Common.Models;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Application.Requests;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Interfaces;
using MediatR;

namespace IdentityUser.src.Application.Handler
{
    public sealed class GetAllUserHandler : IRequestHandler<GetAllUserCommand, GetAllUserQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetAllUserCommand> _validator;

        public GetAllUserHandler(IUserRepository userRepository, IMapper mapper, IValidator<GetAllUserCommand> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetAllUserQuery> Handle(GetAllUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request);
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
}
