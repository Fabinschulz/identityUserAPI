using IdentityUser.src.Domain.Common;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Enums;
using System.Text.Json.Serialization;

namespace IdentityUser.src.Application.Queries.DTOs
{
    /// <summary>
    /// Data Transfer Object for User entity.
    /// </summary>
    public class UserDto : BaseEntity
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        [JsonConverter(typeof(RoleEnumConverter))]
        public RoleEnum? Role { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDto"/> class.
        /// </summary>
        /// <param name="user">The user entity to map from.</param>
        public UserDto(User user)
        {
            Username = user.Username;
            Email = user.Email;
            Role = user.Role;
            Id = user.Id;
            CreatedAt = user.CreatedAt;
            UpdatedAt = user.UpdatedAt;
            DeletedAt = user.DeletedAt;
            IsDeleted = user.IsDeleted;
        }
    }
}
