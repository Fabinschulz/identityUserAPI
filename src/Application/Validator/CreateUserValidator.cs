using FluentValidation;
using IdentityUser.src.Application.Requests;

namespace IdentityUser.src.Application.Validator
{
    public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email é obrigatório.").ChildRules(email =>
            {
                email.RuleFor(x => x).EmailAddress().WithMessage("Email inválido.");
                email.RuleFor(x => x).MaximumLength(50).WithMessage("Email deve ter no máximo 50 caracteres.");
            });
            RuleFor(x => x.Password).NotEmpty().WithMessage("Senha é obrigatória.").ChildRules(password =>
            {
                password.RuleFor(x => x).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
                .WithMessage("Senha deve ter no mínimo 8 caracteres, uma letra maiúscula, uma letra minúscula, um número e um caractere especial.");
            });
        }
    }
}
