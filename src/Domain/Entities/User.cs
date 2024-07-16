using IdentityUser.src.Domain.Common;
using IdentityUser.src.Domain.Enums;

namespace IdentityUser.src.Domain.Entities
{
    public class User : BaseEntity
    {
        public string? Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public RoleEnum? Role { get; set; } =  RoleEnum.User;
        public string Token { get; set; } = string.Empty;


        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }

    }
}
