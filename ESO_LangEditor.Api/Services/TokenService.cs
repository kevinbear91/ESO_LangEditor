using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using ESO_LangEditor.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace ESO_LangEditor.API.Services
{
    //From https://code-maze.com/using-refresh-tokens-in-asp-net-core-authentication/

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public TokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }


        public async Task<string> GenerateAccessToken(User user)
        {
            var tokenConfigSection = _configuration.GetSection("Security:Token");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"]));
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claim = await GetUserClaim(user);

            var jwtToken = new JwtSecurityToken(
                issuer: tokenConfigSection["Issuer"],
                audience: tokenConfigSection["Audience"],
                claims: claim,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: signCredential);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return tokenString;
        }

        public async Task<string> GenerateRefreshToken(User user)
        {
            //var tokenConfigSection = _configuration.GetSection("Security:Token");
            var tokenProvider = "RefreshTokenProvider";
            var tokenPurpose = "RefreshToken";

            var refreshToken = await _userManager.GenerateUserTokenAsync(user, tokenProvider, tokenPurpose);

            return refreshToken;

            //var randomNumber = new byte[32];
            //using (var rng = RandomNumberGenerator.Create())
            //{
            //    rng.GetBytes(randomNumber);
            //    return Convert.ToBase64String(randomNumber);
            //}
        }

        public async Task<bool> VerifyRefreshToken(User user, string token)
        {
            //var tokenConfigSection = _configuration.GetSection("Security:Token");
            var tokenProvider = "RefreshTokenProvider";
            var tokenPurpose = "RefreshToken";

            bool isVaild = await _userManager.VerifyUserTokenAsync(user, tokenProvider, tokenPurpose, token);

            return isVaild;
        }

        public string GenerateRegistrationCode()
        {
            var randomNumber = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenConfigSection = _configuration.GetSection("Security:Token");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
                 
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            //principal.Identity.Name = 
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private async Task<List<Claim>> GetUserClaim(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var roleItem in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, roleItem));
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    //new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString())
                };

            claims.AddRange(userClaims);

            return claims;

        }

        public string GenerateRandomPassword()
        {
            string charRange = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ,.#@!$%";
            string randomWord = "";

            Random rng = new Random();
            int iRandom;

            for (int i = 0; i < 16; i++)
            {
                iRandom = rng.Next(charRange.Length);
                randomWord += charRange[iRandom];
            }
            return randomWord;
        }
    }
}
