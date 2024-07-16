using IdentityUser.src.Domain.Enums;
using System.Text.Json.Serialization;

namespace IdentityUser.src.Application.Queries
{
    public sealed record UpdateUserQuery
    {
        public Guid Id { get; init; }
        public string Username { get; init; } = null!;
        public string Email { get; init; } = null!;

        [JsonConverter(typeof(RoleEnumConverter))]
        public RoleEnum Role { get; init; }
        public bool IsDeleted { get; init; }
    }
}
