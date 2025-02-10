using IdentityUser.src.Domain.Enums;
using System.Text.Json.Serialization;

namespace IdentityUser.src.Application.Queries
{
    /// <summary>
    /// Represents a query to update a user.
    /// </summary>
    public sealed record UpdateUserQuery
    {
        /// <summary>
        /// Gets the unique identifier of the user.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets the username of the user.
        /// </summary>
        public string Username { get; init; } = null!;

        /// <summary>
        /// Gets the email of the user.
        /// </summary>
        public string Email { get; init; } = null!;

        /// <summary>
        /// Gets the role of the user.
        /// </summary>
        [JsonConverter(typeof(RoleEnumConverter))]
        public RoleEnum Role { get; init; }

        /// <summary>
        /// Gets a value indicating whether the user is deleted.
        /// </summary>
        public bool IsDeleted { get; init; }
    }
}
