//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace ServanaAPP.Helpers.JWT
//{
//    public static class TokenHelper
//    {
//        public static string GenerateJWTToken(string userId,string roleName) 
//        {
//            // Initializa Handler
//            var JWTTokenHandler = new JwtSecurityTokenHandler();
//            // SetUp TokenKey
//            // 1- Long Secret  2- Convert String To Bytes
//            string secret = "LongPrimarySecretForServanaApplicationASPCoreModuleForDevelopmentPurpose";
//            var tokenBytesKey = Encoding.UTF8.GetBytes(secret);
//            //  Setup Token Descriptor (Clims, Expiry, Signature)
//            var descriptor = new SecurityTokenDescriptor
//            {
//                Expires = DateTime.Now.AddHours(2),
//                Subject = new ClaimsIdentity(new Claim[]
//                {
//                    new Claim("UserId",userId),
//                    new Claim("Role", roleName)
//                }),
//                SigningCredentials=new SigningCredentials(
//                    new SymmetricSecurityKey(tokenBytesKey), SecurityAlgorithms.HmacSha256Signature)
//            };
//            //Encoding Data Into JSOn Format:
//            var tokenJSON = JWTTokenHandler.CreateToken(descriptor);
//            //Encoding JSOn Result as TOKEN string
//            var token=JWTTokenHandler.WriteToken(tokenJSON);
//            return token;
//        }
       
//        // This is the best way to check.
//        public static string IsValidToken(string token) {
//            try
//            {
//                // Initializa Handler
//                var JWTTokenHandler = new JwtSecurityTokenHandler();
//                // SetUp TokenKey
//                // 1- Long Secret  2- Convert String To Bytes
//                string secret = "LongPrimarySecretForBrainstormingFoodTekApplicationASPCoreModuleForDevelopmentPurpose";
//                var tokenBytesKey = Encoding.UTF8.GetBytes(secret);

//                var tokenValidatorParams = new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(tokenBytesKey),
//                    ValidateLifetime = true,
//                    ValidateIssuer =false,
//                    ValidateAudience = false,
//                    ClockSkew = TimeSpan.Zero
//                };
//                var tokenBase = JWTTokenHandler.ValidateToken(token, tokenValidatorParams, out SecurityToken validateToken);
//                return "Valid";
//            }
//            catch (Exception ex)
//            {
//                return $"Invalid: {ex.Message}";
                
//            }

//        }

//        // This is the worst scenario cuz If hacker edit on token Manually from the jwt.io website it will accept it.
//        public static bool IsValidTokenNotSecured(string token, string requiredRole)
//        {
//            // check if null
//            if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
//            {
//                return false;
//            }
//            // decode the received token
//            var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
//            // check Expiration date
//            if (decodedToken.ValidTo > DateTime.UtcNow)
//            {
//                // Token Not Expired
//                var userRole = decodedToken.Claims.FirstOrDefault(c => c.Type == "Role");
//                if (userRole != null)
//                {
//                    if (userRole.Value != null && userRole.Value.Equals(requiredRole))
//                    {
//                        return true;
//                    }
//                }
//                return false;
//            }
//            else
//            {
//                // Token Expired
//                return false;
//            }

//        }
//    }
//}
