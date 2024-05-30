using IdentityUser.src.Domain.Common;

namespace IdentityUser.src.Domain.Entities
{
    public class User : BaseEntity
    {
        public string? Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; } = "User";
        public string Token { get; set; } = string.Empty;


        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }

    }
}
