using System.ComponentModel.DataAnnotations;
using FluentValidation;
using IdentityUser.src.Application.Queries;
using IdentityUser.src.Domain.Enums;
using MediatR;

namespace IdentityUser.src.Application.Command
{
    /// <summary>
    /// Command to retrieve all users with specified filters and pagination.
    /// </summary>
    /// <param name="Page">The page number for pagination.</param>
    /// <param name="Size">The number of items per page.</param>
    /// <param name="Username">The username to filter by (optional).</param>
    /// <param name="Email">The email to filter by (optional).</param>
    /// <param name="IsDeleted">Indicates whether to include deleted users.</param>
    /// <param name="OrderBy">The field to order the results by (optional).</param>
    /// <param name="Role">The role to filter by (optional).</param>
    public sealed record GetAllUserCommand(
        int Page,
        int Size,
        string? Username,
        string? Email,
        bool IsDeleted,
        string? OrderBy,
        RoleEnum? Role)
        : IRequest<GetAllUserQuery>;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllUserValidator"/> class.
    /// </summary>
    public sealed class GetAllUserValidator : AbstractValidator<GetAllUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllUserValidator"/> class.
        /// </summary>
        /// <remarks>
        /// This validator ensures that the Page property is greater than or equal to 0,
        /// the Size property is not empty and greater than 0, and the Email property is valid.
        /// </remarks>
        public GetAllUserValidator()
        {
            RuleFor(x => x.Page)
               .GreaterThanOrEqualTo(0).WithMessage("A página precisa ser maior ou igual a 0");


            RuleFor(x => x.Size)
                .NotEmpty().WithMessage("Size é obrigatório")
                .GreaterThan(0).WithMessage("Size precisa ser maior que 0");


            RuleFor(x => x.Email)
                .Must(email => IsEmailValid(email!)).WithMessage("Email inválido.");

        }

        private bool IsEmailValid(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}
