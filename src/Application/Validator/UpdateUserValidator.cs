using FluentValidation;
using IdentityUser.src.Application.Requests;

namespace IdentityUser.src.Application.Validator
{
    public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            //RuleFor(x => x.Role).NotEmpty().IsInEnum();
        }
    }
}
