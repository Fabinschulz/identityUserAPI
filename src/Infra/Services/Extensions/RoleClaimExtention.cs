using IdentityUser.src.Domain.Entities;
using System.Security.Claims;

namespace IdentityUser.src.Infra.Services.Extensions
{
    public static class RoleClaimExtention
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>
            {
                new ("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username?.ToString() ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email.ToString()),
                new Claim(ClaimTypes.Role, user.Role?.ToString() ?? string.Empty),
            };
            return result;
        }
    }
}
