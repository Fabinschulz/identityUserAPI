using System.Security.Cryptography;

namespace IdentityUser.src.Infra.Services.TokenServices
{
    public static class JwtKeySettings
    {
        public static string JwtKey { get; } = GenerateJwtKey();

        private static string GenerateJwtKey()
        {
            const int keySize = 32; // Tamanho da chave em bytes

            using (var rng = new RNGCryptoServiceProvider())
            {
                var keyBytes = new byte[keySize];
                rng.GetBytes(keyBytes);
                return Convert.ToBase64String(keyBytes);
            }
        }
    }
}
