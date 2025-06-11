using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using ServanaAPP.Models;

namespace ServanaAPP.Helpers.JWT
{
    public class GenerateJwtTokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public GenerateJwtTokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        }

        public string CreateToken(User user)
        {

            var claims = new List<Claim>
{
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim("FullName", user.FullName),
            new Claim("Role", user.Role.ToString())
};

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:IssuerIP"],
                audience: _configuration["JWT:AudienceIP"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
