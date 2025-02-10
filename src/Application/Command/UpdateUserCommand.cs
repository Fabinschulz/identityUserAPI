using FluentValidation;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Enums;
using MediatR;
using System.Text.Json.Serialization;

namespace IdentityUser.src.Application.Command
{
    /// <summary>
    /// Command to update a user's information.
    /// </summary>
    /// <param name="Id">The unique identifier of the user.</param>
    /// <param name="Username">The username of the user.</param>
    /// <param name="Email">The email address of the user.</param>
    /// <param name="Role">The role of the user, converted using <see cref="RoleEnumConverter"/>.</param>
    /// <param name="IsDeleted">Indicates whether the user is marked as deleted.</param>
    public sealed record UpdateUserCommand(
        Guid Id,
        string Username,
        string Email,
        [property: JsonConverter(typeof(RoleEnumConverter))] RoleEnum Role,
        bool IsDeleted
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
            RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            //RuleFor(x => x.Role).NotEmpty().IsInEnum();
        }
    }
}
