using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend_teamwork1.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace backend_teamwork1.Services
{
    public class AuthService
    {
        public AuthService()
        {
            Console.WriteLine($"JWT Issuer: {Environment.GetEnvironmentVariable("Jwt__Issuer")}");
        }

        public string GenerateJwt(UserDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key") ?? throw new InvalidOperationException("JWT Key is missing in environment variables.");
            var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer") ?? throw new InvalidOperationException("JWT Issuer is missing in environment variables.");
            var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience") ?? throw new InvalidOperationException("JWT Audience is missing in environment variables.");

            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                   new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                   new Claim(ClaimTypes.Role, "admin")
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
