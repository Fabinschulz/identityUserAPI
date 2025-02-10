using FluentValidation;
using IdentityUser.src.Application.Queries;
using MediatR;

namespace IdentityUser.src.Application.Command
{
    /// <summary>
    /// Command to log in a user.
    /// </summary>
    /// <param name="Email">The email address of the user.</param>
    /// <param name="Password">The password of the user.</param>
    public sealed record LoginUserCommand(string Email, string Password) : IRequest<LoginUserQuery>;

    /// <summary>
    /// Validator for the <see cref="LoginUserCommand"/> class.
    /// </summary>
    public sealed class LoginUserValidator : AbstractValidator<LoginUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginUserValidator"/> class.
        /// Sets up validation rules for the <see cref="LoginUserCommand"/> properties.
        /// </summary>
        public LoginUserValidator()
        {
            // Validation rules
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email é obrigatório.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Senha é obrigatória.");
        }
    }
}
