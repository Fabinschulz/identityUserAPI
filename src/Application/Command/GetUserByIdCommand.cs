using FluentValidation;
using IdentityUser.src.Application.Queries;
using MediatR;

namespace IdentityUser.src.Application.Command
{
    /// <summary>
    /// Command to get a user by their unique identifier.
    /// </summary>
    /// <param name="Id">The unique identifier of the user.</param>
    public sealed record GetUserByIdCommand(Guid Id) : IRequest<GetUserByIdQuery>;

    /// <summary>
    /// Validator for the <see cref="GetUserByIdCommand"/> class.
    /// </summary>
    public sealed class GetUserByIdValidator : AbstractValidator<GetUserByIdCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserByIdValidator"/> class.
        /// </summary>
        public GetUserByIdValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty().WithMessage("Id is required")
               .Must(id => IsGuidValid(id)).WithMessage("Id is not a valid GUID");
        }

        /// <summary>
        /// Validates whether the provided GUID is valid.
        /// </summary>
        /// <param name="id">The GUID to validate.</param>
        /// <returns>True if the GUID is valid; otherwise, false.</returns>
        private bool IsGuidValid(Guid id)
        {
            return Guid.TryParse(id.ToString(), out _);
        }
    }
}
