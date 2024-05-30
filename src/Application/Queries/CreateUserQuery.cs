using IdentityUser.src.Domain.Entities;

namespace IdentityUser.src.Application.Queries
{
    public sealed record CreateUserQuery
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public static implicit operator CreateUserQuery(User user)
        {
            return new CreateUserQuery
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
