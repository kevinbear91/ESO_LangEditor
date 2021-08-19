using AutoMapper;
using ESO_LangEditor.API.Services;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private ITokenService _tokenService;
        private IMapper _mapper;
        private ILogger<AdminController> _logger;
        private IConfiguration _configuration;
        private IRepositoryWrapper _repositoryWrapper;

        public AdminController(RoleManager<Role> roleManager, UserManager<User> userManager, 
            IMapper mapper, ITokenService tokenService, ILogger<AdminController> logger,
            IConfiguration configuration, IRepositoryWrapper repositoryWrapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
            _configuration = configuration;
            _repositoryWrapper = repositoryWrapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> UserList()
        {
            var users = _userManager.Users.ToList();

            var usersDto = _mapper.Map<List<UserDto>>(users);

            return usersDto;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/{userGuid}")]
        public async Task<ActionResult<UserDto>> GetUser(string userGuid)
        {
            var user = await _userManager.FindByIdAsync(userGuid);

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var userDto = _mapper.Map<UserDto>(user);

            userDto.UserRoles = userRoles.ToList();

            return userDto;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("roles/{userGuid}")]
        public async Task<IActionResult> ModifyUserRolesAsync(string userGuid, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userGuid);
            var userIdFromToken = _userManager.GetUserId(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (user == null)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserNotFound,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserNotFound)
                });
            }

            var adminSetting = _configuration.GetSection("Admin:Setting");
            string createrId = adminSetting["Creater"];

            if (user.Id == new Guid(createrId) && userIdFromToken != createrId)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserRoleSetFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserRoleSetFailed)
                });
            }

            var roleInServer = _roleManager.Roles.ToDictionary(r => r.Name);

            var userInRoles = await _userManager.GetRolesAsync(user);


            var roleForAdd = roles;

            foreach (var role in roleInServer)
            {
                if (await _userManager.IsInRoleAsync(user, role.Key))
                {
                    if (!roles.Contains(role.Key))
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Key);
                    }
                    
                    roleForAdd.Remove(role.Key);
                }
            }

            if (roleForAdd.Count == 0)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserRoleSetFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserRoleSetFailed)
                });
            }

            var result = await _userManager.AddToRolesAsync(user, roleForAdd);

            if (result.Succeeded)
            {
                return Ok(new MessageWithCode
                {
                    Code = (int)RespondCode.Success,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.Success)
                });
            }
            else
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserRoleSetFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserRoleSetFailed)
                });
            }

            

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("role")]
        public async Task<ActionResult<bool>> AddUserRoleAsync(string role)
        {
            //var user = await _userManager.FindByIdAsync(userGuid);
            var userIdFromToken = _userManager.GetUserId(HttpContext.User);

            if (await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.RoleExisted,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.RoleExisted)
                });
            }


            var result = await _roleManager.CreateAsync(new Role { Name = role });

            if (result.Succeeded)
            {
                return Ok(new MessageWithCode
                {
                    Code = (int)RespondCode.Success,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.Success)
                });
            }
            else
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.RoleAddFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.RoleAddFailed)
                });
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("passwordRecoveryCode/{userId}")]
        public async Task<ActionResult<string>> GetUserPasswordRecoveryToken(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());



            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Ok(token);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("setPasswordRandom/{userId}")]
        public async Task<IActionResult> SetUserPasswordToRandom(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (!await _userManager.HasPasswordAsync(user))
            {
                await _userManager.AddPasswordAsync(user, Guid.NewGuid().ToString());
            }


            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            //string randomPassword = _tokenService.GenerateRandomPassword();
            var result = await _userManager.ResetPasswordAsync(user, resetToken, Guid.NewGuid().ToString());

            await _tokenService.GenerateRefreshToken(user);

            if (result.Succeeded)
            {
                return Ok(new MessageWithCode
                {
                    Code = (int)RespondCode.Success,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.Success)
                });
            }
            else
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.PasswordChangeFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.PasswordChangeFailed)
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("modifyUser/{userId}")]
        public async Task<IActionResult> ModifyUserInfo(Guid userId, SetUserInfoDto setUserInfoDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var userIdFromToken = _userManager.GetUserId(HttpContext.User);

            if (user.Id != userId && user.Id != setUserInfoDto.UserID)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserNotFound,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserNotFound)
                });
            }

            var adminSetting = _configuration.GetSection("Admin:Setting");
            string createrId = adminSetting["Creater"];

            if (user.Id == new Guid(createrId) && userIdFromToken != createrId)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserUpdateFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserUpdateFailed)
                });
            }

            user.UserName = setUserInfoDto.UserName;
            user.UserNickName = setUserInfoDto.UserNickName;

            if (setUserInfoDto.LockoutEnabled)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.MaxValue;
            }
            else
            {
                user.LockoutEnabled = false;
                user.LockoutEnd = DateTimeOffset.UtcNow;
            }

            var result = await _userManager.UpdateAsync(user);

            var userRev = await _repositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(2);
            userRev.Rev++;
            _repositoryWrapper.LangTextRevNumberRepo.Update(userRev);

            

            if (result.Succeeded)
            {
                await _repositoryWrapper.LangTextRevNumberRepo.SaveAsync();

                return Ok(new MessageWithCode
                {
                    Code = (int)RespondCode.Success,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.Success)
                });
            }
            else
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserUpdateFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserUpdateFailed)
                });
            }
        }


        //[Authorize(Roles = "Admin")]
        //[HttpPost("register")]
        //public async Task<ActionResult<UserDto>> AddUserAsync(UserDto username)
        //{
        //    Guid userId;

        //    if (username.ID == null || username.ID == new Guid())
        //    {
        //        userId = Guid.NewGuid();
        //    }
        //    else
        //    {
        //        userId = username.ID;
        //    }

        //    var user = new User
        //    {
        //        Id = userId,
        //        UserName = username.UserName,
        //        //Email = "123@2233.com",

        //    };

        //    var result = await _userManager.CreateAsync(user);

        //    //foreach(var error in result.Errors)
        //    //{
        //    //    Debug.WriteLine(error.Description);
        //    //    _loggerMessage = "Creat user Failed, Error: " + error;
        //    //    _logger.LogError(_loggerMessage);
        //    //}

        //    if (result.Succeeded)
        //    {

        //        await _userManager.AddToRoleAsync(user, "InitUser");

        //        var refreshToken = _tokenService.GenerateRefreshToken();
        //        user.RefreshToken = refreshToken;
        //        user.RefreshTokenExpireTime = DateTime.Now.AddDays(1);

        //        await _userManager.UpdateAsync(user);
        //        var userDto = _mapper.Map<UserDto>(user);

        //        return userDto;

        //    }
        //    else
        //    {
        //        foreach (var error in result.Errors)
        //        {
        //            Debug.WriteLine(error.Description);
        //            _loggerMessage = "Create user Failed, Error: " + error;
        //            _logger.LogError(_loggerMessage);
        //        }
        //        return BadRequest();
        //    }

        //}

        //[Authorize(Roles = "Creater")]
        //[HttpGet("init/{userId}")]
        //public async Task<ActionResult<UserDto>> InitExistUserAsync(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);

        //    if (user.SecurityStamp == null)
        //    {
        //        await _userManager.UpdateSecurityStampAsync(user);
        //    }

        //    if (user != null)
        //    {
        //        await _userManager.AddToRoleAsync(user, "InitUser");

        //        var refreshToken = _tokenService.GenerateRefreshToken();
        //        user.RefreshToken = refreshToken;
        //        user.RefreshTokenExpireTime = DateTime.Now.AddDays(1);

        //        await _userManager.UpdateAsync(user);
        //        var userDto = _mapper.Map<UserDto>(user);

        //        return userDto;

        //    }
        //    else
        //    {
        //        _loggerMessage = "Init user Failed, user id: " + userId;
        //        _logger.LogError(_loggerMessage);
        //        return BadRequest();
        //    }

        //}
    }
}
