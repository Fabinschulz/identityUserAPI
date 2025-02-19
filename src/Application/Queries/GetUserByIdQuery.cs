using System.Text.Json.Serialization;
using IdentityUser.src.Domain.Common;
using IdentityUser.src.Domain.Enums;

namespace IdentityUser.src.Application.Queries
{

    /// <summary>
    /// Represents a query to get a user by their unique identifier.
    /// </summary>
    public sealed class GetUserByIdQuery : BaseEntity
    {
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
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RoleEnum? Role { get; set; }

    }

}
