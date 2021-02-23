using ESO_LangEditor.API.Services;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {

        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private RoleManager<Role> _roleManager;
        private ITokenService _tokenService;

        public AccountController(UserManager<User> userManager,
          SignInManager<User> signInManager, ITokenService tokenService,
          RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }

        [HttpPost("register", Name = nameof(AddUserAsync))]
        public async Task<IActionResult> AddUserAsync(RegisterUser registerUser)
        {
            var user = new User
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,

            };

            IdentityResult result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(user, "Editor");
                return Ok();
            }
            else
            {
                ModelState.AddModelError("Error", result.Errors.FirstOrDefault()?.Description);
                return BadRequest(ModelState);
            }

        }

        [HttpPost("login", Name = nameof(Login))]
        public async Task<IActionResult> Login(LoginUserDto loginUser)
        {

            //var result = await _signInManager.PasswordSignInAsync(
            //        loginUser.UserName, loginUser.Password, true, false);

            //var user = await _userManager.FindByNameAsync(loginUser.UserName);
            var user = await _userManager.FindByIdAsync(loginUser.UserID.ToString());

            if (user == null)
            {
                return Unauthorized();
            }


            if (!await _userManager.CheckPasswordAsync(user, loginUser.Password))
            {
                return Unauthorized();
            }

            //await _userManager.GetRolesAsync(user)


            //if (!result.Succeeded)
            //{
            //    //return RedirectToAction("index"，"home");
            //    return BadRequest();
            //}

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var roleItem in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, roleItem));
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    //new Claim(claims., Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString())
                };

            claims.AddRange(userClaims);

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, loginUser.UserName),
            //    new Claim(ClaimTypes., loginUser.UserName),
            //    //new Claim(ClaimTypes.Role, await _userManager.GetRolesAsync(user))
            //};
            var authToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireTime = DateTime.Now.AddDays(7);
            //userContext.SaveChanges();

            await _userManager.UpdateAsync(user);

            return Ok(new TokenDto
            {
                AuthToken = authToken,
                RefreshToken = refreshToken
            });

            //ModelState.AddModelError(string.Empty, "登录失败，请重试");
            


            //return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();

            return Ok();
            //return RedirectToAction("index"，"home");
        }

    }
}
