using IdentityUser.src.Domain.Common;
using IdentityUser.src.Domain.Enums;
using System.Text.Json.Serialization;

namespace IdentityUser.src.Domain.Entities
{
    /// <summary>
    /// Represents a user entity.
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string? Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        [JsonConverter(typeof(RoleEnumConverter))]
        public RoleEnum? Role { get; set; } = RoleEnum.User;

        /// <summary>
        /// Gets or sets the token of the user.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with the specified email and password.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
