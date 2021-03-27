using ESO_LangEditor.API.Services;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private IConfiguration _configuration;
        private RoleManager<Role> _roleManager;
        private UserManager<User> _userManager;
        private IRepositoryWrapper _repositoryWrapper;
        private SignInManager<User> _signInManager;
        private ITokenService _tokenService;

        public AuthenticateController(UserManager<User> userManager, RoleManager<Role> roleManager,
            IConfiguration configuration, SignInManager<User> signInManager, ITokenService tokenService,
            IRepositoryWrapper repositoryWrapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _repositoryWrapper = repositoryWrapper;
            _configuration = configuration;
            _tokenService = tokenService;
        }




        //[HttpPost("token2", Name = nameof(GenerateTokenAsync))]
        //public async Task<IActionResult> GenerateTokenAsync(LoginUserDto loginUser)
        //{
        //    var user = await UserManager.FindByNameAsync(loginUser.UserName);

        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }


        //    var result = UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);

        //    if (result != PasswordVerificationResult.Success)
        //    {
        //        return Unauthorized();
        //    }

        //    var userClaims = await UserManager.GetClaimsAsync(user);
        //    var userRoles = await UserManager.GetRolesAsync(user);
        //    foreach (var roleItem in userRoles)
        //    {
        //        userClaims.Add(new Claim(ClaimTypes.Role, roleItem));
        //    }

        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Email, user.Email)
        //    };

        //    claims.AddRange(userClaims);


        //}


        [HttpPost("refresh", Name = nameof(RefreshToken))]
        public async Task<ActionResult> RefreshToken(TokenDto tokenDto)
        {
            if (tokenDto is null)
            {
                return BadRequest("Invalid client request");
            }
            string authToken = tokenDto.AuthToken;
            string refreshToken = tokenDto.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(authToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            //var user = userContext.LoginModels.SingleOrDefault(u => u.UserName == username);
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }
            var newAuthTokenToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                authToken = newAuthTokenToken,
                refreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost("revoke", Name = nameof(RevokeToken))]
        public async Task<ActionResult> RevokeToken()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }


    }
}
