using FluentValidation;
using IdentityUser.src.Application.Requests;

namespace IdentityUser.src.Application.Validator
{
    public sealed class GetUserByIdValidator : AbstractValidator<GetUserByIdCommand>
    {
        public GetUserByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .Must(id => IsGuidValid(id)).WithMessage("Id is not a valid GUID");
        }

        private bool IsGuidValid(Guid id)
        {
            return Guid.TryParse(id.ToString(), out _);
        }
    }
}
