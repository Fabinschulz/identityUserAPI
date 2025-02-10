using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Infra.Services.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityUser.src.Infra.Services.TokenServices
{
    /// <summary>
    /// Provides methods for generating tokens.
    /// </summary>
    public static class TokenService
    {

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <returns>A JWT token as a string.</returns>
        public static string GenerateToken(User user)
        {
            //Estancia do manipulador de Token
            var tokenHandler = new JwtSecurityTokenHandler();
            //Chave da classe Settings. O Token Handler espera um Array de Bytes, por isso é necessário converter
            var key = Encoding.ASCII.GetBytes(JwtKeySettings.JwtKey);

            var claims = user.GetClaims();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // Claims que vão compor o token
                Expires = DateTime.UtcNow.AddHours(8), // Tempo de expiração do token
                //Assinatura do token, serve para identificar quem mandou o token e garantir que o token não foi alterado no meio do caminho.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            //Gerando o token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
