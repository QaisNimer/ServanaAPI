//using Microsoft.IdentityModel.JsonWebTokens;
//using Microsoft.IdentityModel.Tokens;
//using System.Security.Claims;
//using System.Text;
//using System;
//using ServanaAPP.Interfaces;
//using ServanaAPP.Models;

//namespace BrainstormingFoodTek.Helpers.JWT

//{
//    public class TokenProviderHelper: ITokenProvider
//    {
//        private readonly IConfiguration _configuration;

//        public TokenProviderHelper(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }
//        public string CreateToken(User user)
//        {
//            string secretKey = _configuration["JWT:Secret"]!;
//            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new[]
//                {
//                    new Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
//                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
//                    new Claim("email_verified", user.Email.ToString())
//                }),
//                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JWT:ExpirationInMinutes")),
//                SigningCredentials = credentials,
//                Issuer = _configuration["JWT:Issuer"],
//                Audience = _configuration["JWT:Audience"]

//            };

//            var handler = new JsonWebTokenHandler();
//            string token = handler.CreateToken(tokenDescriptor);

//            return token;
//        }
//    }
//}
