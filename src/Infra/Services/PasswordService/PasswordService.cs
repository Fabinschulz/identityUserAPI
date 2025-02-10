using System.Security.Cryptography;

namespace IdentityUser.src.Infra.Services.PasswordService
{
    /// <summary>
    /// Provides methods for hashing and verifying passwords.
    /// </summary>
    public class PasswordService
    {
        private const int SaltSize = 16;
        private const int HashSize = 20; // Tamanho do hash SHA-1

        /// <summary>
        /// Creates a hash from the provided password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hashed password as a base64 string.</returns>
        public static string HashPassword(string password)
        {
            byte[] salt;
            RandomNumberGenerator.Create().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verifies if the provided password matches the stored password hash.
        /// </summary>
        /// <param name="savedPasswordHash">The stored password hash.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns>True if the password matches the hash; otherwise, false.</returns>
        public static bool VerifyPasswordHash(string savedPasswordHash, string password)
        {
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);
                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                        return false;
                }
                return true;
            }
        }

    }
}
