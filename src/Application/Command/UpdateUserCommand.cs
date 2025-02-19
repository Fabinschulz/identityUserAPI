using FluentValidation;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Entities;
using MediatR;

namespace IdentityUser.src.Application.Command
{
    /// <summary>
    /// Command to update a user's information.
    /// </summary>
    /// <param name="Id">The unique identifier of the user.</param>
    /// <param name="user">The user to be updated.</param>
    public sealed record UpdateUserCommand(
        Guid Id,
        User user
    ) : IRequest<UpdateUserQuery>;

    /// <summary>
    /// Validator for the <see cref="UpdateUserCommand"/> class.
    /// </summary>
    public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserValidator"/> class.
        /// </summary>
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.user).NotNull().ChildRules(user =>
            {
                user.RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
                user.RuleFor(x => x.Email).NotEmpty().EmailAddress();
                user.RuleFor(x => x.Role).IsInEnum();
            });
        }
    }
}
