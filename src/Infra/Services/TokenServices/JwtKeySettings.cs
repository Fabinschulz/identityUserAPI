using System.Security.Cryptography;

namespace IdentityUser.src.Infra.Services.TokenServices
{
    /// <summary>
    /// Provides settings for JWT key generation.
    /// </summary>
    public static class JwtKeySettings
    {
        /// <summary>
        /// Gets the JWT key.
        /// </summary>
        public static string JwtKey { get; } = GenerateJwtKey();

        private static string GenerateJwtKey()
        {
            const int keySize = 32; // Tamanho da chave em bytes

            using (var rng = RandomNumberGenerator.Create())
            {
                var keyBytes = new byte[keySize];
                rng.GetBytes(keyBytes);
                return Convert.ToBase64String(keyBytes);
            }
        }
    }
}
