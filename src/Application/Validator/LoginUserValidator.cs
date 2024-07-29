using FluentValidation;
using IdentityUser.src.Application.Requests;

namespace IdentityUser.src.Application.Validator
{
    public sealed class LoginUserValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserValidator()
        {

            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email é obrigatório.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Senha é obrigatória.");
        }
    }
}
