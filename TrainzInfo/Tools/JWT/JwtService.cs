using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TrainzInfo.Tools.JWT
{
    public class JwtService
    {
        private readonly string _secret;
        private readonly string _issuer;
        public JwtService(string secret, string issuer)
        {
            _secret = secret;
            _issuer = issuer;
        }

        public string GenerateToken(IdentityUser user, IList<string> roles)
        {
            LoggingExceptions.Init(this.ToString(), nameof(GenerateToken));
            LoggingExceptions.Start();

            LoggingExceptions.Wright("Set claims");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Додаємо ролі
            foreach (var role in roles)
            {
                LoggingExceptions.Wright($"Add role claim: {role}");
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );
            LoggingExceptions.Wright("Token generated successfully");
            LoggingExceptions.Finish();
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
