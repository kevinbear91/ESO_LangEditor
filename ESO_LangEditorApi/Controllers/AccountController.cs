using AutoMapper;
using ESO_LangEditor.API.Services;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
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
        private IWebHostEnvironment _webHostEnvironment;
        private IMapper _mapper;

        public AccountController(UserManager<User> userManager,
          SignInManager<User> signInManager, ITokenService tokenService,
          RoleManager<Role> roleManager, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> AddUserAsync(RegisterUser registerUser)
        //{
        //    //if (_userManager.Users.Count() >= 1)
        //    //{
        //    //    return BadRequest();
        //    //}

        //    var user = new User
        //    {
        //        Id = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
        //        UserName = registerUser.UserName,
        //        Email = registerUser.Email,
        //    };

        //    IdentityResult result = await _userManager.CreateAsync(user, registerUser.Password);

        //    if (result.Succeeded)
        //    {
        //        List<string> roles = new List<string>
        //        {
        //            "Editor",
        //            "Reviewer",
        //            "Admin",
        //            "Creater"
        //        };
        //        var userCreated = await _userManager.FindByIdAsync(user.Id.ToString());

        //        await _userManager.AddToRolesAsync(userCreated, roles);
        //        return Ok();
                
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("Error", result.Errors.FirstOrDefault()?.Description);
        //        return BadRequest(ModelState);
        //    }

        //}

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

            if (user.UserName == null && user.UserNickName == null)
            {
                return BadRequest(CustomRespondCode.InitAccountRequired);
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
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    //new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString())
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

        [HttpPost("FirstTimeLogin", Name = nameof(FirstTimeLogin))]
        public async Task<IActionResult> FirstTimeLogin(LoginUserDto loginUser)
        {
            var user = await _userManager.FindByIdAsync(loginUser.UserID.ToString());

            if (user == null)
            {
                return Unauthorized();
            }

            if (user.PasswordHash == null 
                && user.UserNickName == null
                && user.RefreshToken == loginUser.RefreshToken)
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
            }

            return BadRequest();

        }

        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();

            return Ok();
            //return RedirectToAction("index"，"home");
        }

        [Authorize]
        [HttpPost("infochange")]
        public async Task<IActionResult> UserChangeInfoAsync(UserInfoChangeDto userInfoChangeDto)
        {
            var user = await _userManager.FindByIdAsync(userInfoChangeDto.UserID.ToString());
            var userIdFromToken = _userManager.GetUserId(HttpContext.User);

            if (user == null || userIdFromToken != user.Id.ToString())
            {
                return Unauthorized();
            }

            if(user.UserName != userInfoChangeDto.UserName && !string.IsNullOrWhiteSpace(userInfoChangeDto.UserName))
            {
                user.UserName = userInfoChangeDto.UserName;
            }

            if (user.UserNickName != userInfoChangeDto.UserNickName && !string.IsNullOrWhiteSpace(userInfoChangeDto.UserNickName))
            {
                user.UserNickName = userInfoChangeDto.UserNickName;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    return BadRequest(error);
                }
            }

            if (!string.IsNullOrWhiteSpace(userInfoChangeDto.NewPassword))
            {
                var PwChangeresult = await _userManager.ChangePasswordAsync(user,
                    userInfoChangeDto.OldPassword,
                    userInfoChangeDto.NewPassword);

                if (PwChangeresult.Succeeded)
                {
                    return Ok();
                }
                foreach (var error in result.Errors)
                {
                    return BadRequest(error);
                }
            }

            return Ok();
            
        }

        [Authorize]
        [HttpPost("infoinit")]
        public async Task<IActionResult> UserInitInfoAsync(UserInfoChangeDto userInfoChangeDto)
        {
            var user = await _userManager.FindByIdAsync(userInfoChangeDto.UserID.ToString());
            var userIdFromToken = _userManager.GetUserId(HttpContext.User);

            if (user == null || userIdFromToken != user.Id.ToString())
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(userInfoChangeDto.NewPassword))
            {
                return Unauthorized();
            }

            user.UserNickName = userInfoChangeDto.UserNickName;
            //user.UserName = userInfoChangeDto.UserName;

            var setNameResult = await _userManager.SetUserNameAsync(user, userInfoChangeDto.UserName);

            if (!setNameResult.Succeeded)
            {
                foreach (var error in setNameResult.Errors)
                {
                    Debug.WriteLine("UserName error: " + error);
                    return BadRequest(error);
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Debug.WriteLine("Update User error: " + error);
                    return BadRequest(error);
                }
            }

            var resultForPw = await _userManager.AddPasswordAsync(user, userInfoChangeDto.NewPassword);

            if (!resultForPw.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Debug.WriteLine(error);
                    return BadRequest(error);
                }
                //return Ok();
            }

            var userAfterSetting = await _userManager.FindByIdAsync(userInfoChangeDto.UserID.ToString());

            if (await _userManager.IsInRoleAsync(userAfterSetting, "InitUser"))
            {
                await _userManager.RemoveFromRoleAsync(userAfterSetting, "InitUser");
                await _userManager.AddToRoleAsync(userAfterSetting, "Editor");
            }

            return Ok();

        }

        [Authorize]
        [HttpPost("{userGuid}/avatar")]
        public async Task<IActionResult> UserUploadAvatarAsync(Guid userGuid, [FromForm] IFormFile avatar)
        {
            var userIdFromToken = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userIdFromToken);
            
            if (user == null && userIdFromToken != userGuid.ToString())
            {
                return Unauthorized();
            }

            //string uniqueFileName = null;

            if (avatar.Length > 512 * 1024) // 512 * 1024 = 512 KB
            {
                return BadRequest();
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwwroot/images");
            string uniqueFileName = user.UserName + "_" + Guid.NewGuid().ToString();
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            Debug.WriteLine("uplpad folder: {0}, filename: {1}, filepath: {2}", uploadsFolder, uniqueFileName, filePath);

            avatar.CopyTo(new FileStream(filePath, FileMode.Create));

            if(!System.IO.File.Exists(filePath))
            {
                return BadRequest();
            }

            user.UserAvatarPath = uniqueFileName;
            await _userManager.UpdateAsync(user);

            return Ok();
            
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<UserInClientDto>>> GetUsersAsync()
        {
            //var user = new User
            //{
            //    UserName = registerUser.UserName,
            //    Email = registerUser.Email,

            //};

            var users = await Task.Run(() => _userManager.Users.ToList());
            var userClientDto = _mapper.Map<List<UserInClientDto>>(users);

            return userClientDto;

        }

        [HttpGet("{userGuid}/roles")]
        public async Task<ActionResult<List<string>>> GetUserRolesAsync(string userGuid)
        {

            var user = await _userManager.FindByIdAsync(userGuid);

            if (user == null)
            {
                return BadRequest();
            }

            List<string> roles = (List<string>)await _userManager.GetRolesAsync(user);

            return roles;
        }

        //[HttpPost]
        //public async Task<IActionResult> InitUserByGuidAsync(Guid userGuid)
        //{
        //    var user = await _userManager.FindByIdAsync(userGuid.ToString());

        //    if (user.PasswordHash.Length < 8 && user.UserName == "" && user.UserNickName == "")
        //    {
        //        return BadRequest("InitUser");
        //    }


        //    return Ok();
        //    //return RedirectToAction("index"，"home");
        //}

    }
}
