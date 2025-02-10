using IdentityUser.src.Domain.Entities;

namespace IdentityUser.src.Application.Queries
{
    /// <summary>
    /// Represents a query to create a user.
    /// </summary>
    public sealed record CreateUserQuery
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public required string Role { get; set; }

        /// <summary>
        /// Implicitly converts a <see cref="User"/> object to a <see cref="CreateUserQuery"/> object.
        /// </summary>
        /// <param name="user">The user to convert.</param>
        /// <returns>A <see cref="CreateUserQuery"/> object.</returns>
        public static implicit operator CreateUserQuery(User user)
        {
            return new CreateUserQuery
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString()!
            };
        }
    }
}
