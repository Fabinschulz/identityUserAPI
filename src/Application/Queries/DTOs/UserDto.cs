using IdentityUser.src.Domain.Common;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Enums;
using System.Text.Json.Serialization;

namespace IdentityUser.src.Application.Queries.DTOs
{
    public class UserDto : BaseEntity
    {
        public string? Username { get; set; }
        public string Email { get; set; }

        [JsonConverter(typeof(RoleEnumConverter))]
        public RoleEnum? Role { get; set; }

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
