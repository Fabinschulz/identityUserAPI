using IdentityUser.src.Domain.Entities;
using System.Security.Claims;

namespace IdentityUser.src.Infra.Services.Extensions
{
    /// <summary>
    /// Provides extension methods for role claims.
    /// </summary>
    public static class UserClaimsExtensions
    {
        /// <summary>
        /// Gets the claims for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to get the claims.</param>
        /// <returns>A collection of claims for the user.</returns>
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>
            {
                new ("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString()),
                new Claim(ClaimTypes.Role, user.Role?.ToString() ?? string.Empty),
            };
            return result;
        }
    }
}
