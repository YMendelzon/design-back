using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DesigneryCore.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly string _accessTokenSecret;
        private readonly string _refreshTokenSecret;
        private readonly double _accessTokenExpiry;
        private readonly double _refreshTokenExpiry;

        private readonly ConcurrentDictionary<string, string> _refreshTokens = new(); // In-memory store

        public TokenService(IConfiguration config)
        {
            _config = config;
            _accessTokenSecret = _config["Jwt:Key"];
            _refreshTokenSecret = _config["Jwt:RefreshTokenSecret"];
            _accessTokenExpiry = Convert.ToDouble(_config["Jwt:AccessTokenExpiryMinutes"]);
            _refreshTokenExpiry = Convert.ToDouble(_config["Jwt:RefreshTokenExpiryDays"]) * 24 * 60; // Convert days to minutes
        }



        // Generate an Access Token
        public string BuildAccessToken(string role, string email)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])), SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiryDurationMinutes"]))
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        // Generate a Refresh Token
        public string BuildRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        // Validate an Access Token
        public bool ValidateAccessToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_accessTokenSecret)),
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var tokenHandler1 = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler1.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return false;

                return true;

            }
            catch
            {
                return false;
            }
        }
        // Validate a Refresh Token
        public bool ValidateRefreshToken(string token)
        {
            // Check if the token exists in the dictionary
            var exists = _refreshTokens.Values.Any(v => v == token);
            return /*await Task.FromResult(*/exists;
        }
        public async Task SaveRefreshToken(string email, string refreshToken)
        {
            _refreshTokens[email] = refreshToken;
            await Task.CompletedTask; // Simulate async operation
        }
        public string GetRefreshToken(string email)
        {
            _refreshTokens.TryGetValue(email, out var refreshToken);
            return refreshToken;
        }

        // Get Email from an Access Token
        public string GetEmailFromAccessToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub);
            return emailClaim?.Value;
        }


    }
}
