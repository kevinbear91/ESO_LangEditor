using AutoMapper;
using ESO_LangEditor.API.Filters;
using ESO_LangEditor.API.Services;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
        private ILogger<AccountController> _logger;
        private string _loggerMessage;
        private IRepositoryWrapper _repositoryWrapper;

        public AccountController(UserManager<User> userManager,
          SignInManager<User> signInManager, ITokenService tokenService,
          RoleManager<Role> roleManager, IMapper mapper, IWebHostEnvironment webHostEnvironment,
          ILogger<AccountController> logger, IRepositoryWrapper repositoryWrapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _repositoryWrapper = repositoryWrapper;
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

        [ServiceFilter(typeof(ValidationFilter))]
        [HttpPost("login", Name = nameof(Login))]
        public async Task<IActionResult> Login(LoginUserDto loginUser)
        {
            var user = await _userManager.FindByNameAsync(loginUser.UserName);

            if (user == null)
            {
                //_loggerMessage = "Can't find user, From client UserName: " + loginUser.UserName;
                _logger.LogInformation(ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserNotFound));
                return NotFound(new MessageWithCode
                {
                    Code = (int)RespondCode.UserNotFound,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserNotFound)
                });
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserLocked,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserLocked)
                });
            }


            if (!await _userManager.CheckPasswordAsync(user, loginUser.Password))

            {
                //_loggerMessage = "User Password not match! UserName: " + loginUser.UserName;
                _logger.LogInformation(ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserNotFound));

                return Unauthorized(new MessageWithCode 
                {
                    Code = (int)RespondCode.PasswordNotMatch,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.PasswordNotMatch)
                });
            }

            var authToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshToken(user);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireTime = DateTime.Now.AddDays(30);

            await _userManager.UpdateAsync(user);

            return Ok(new TokenDto
            {
                AuthToken = authToken,
                RefreshToken = refreshToken
            });
        }

        //[HttpPost]
        //public async Task<IActionResult> LogoutAsync()
        //{
        //    await _signInManager.SignOutAsync();

        //    return Ok();
        //    //return RedirectToAction("index"，"home");
        //}

        [ServiceFilter(typeof(ValidationFilter))]
        [HttpPost("token/{userId}")]
        public async Task<IActionResult> RefreshToken(string userId, TokenDto tokenDto)
        {
            string authToken = tokenDto.AuthToken;
            string refreshToken = tokenDto.RefreshToken;
            //var principal = _tokenService.GetPrincipalFromExpiredToken(authToken);
            //var username = principal.Identity.Name; //this is mapped to the Name claim by default
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserNotFound,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserNotFound)
                });
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserLocked,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserLocked)
                });
            }

            if (user.RefreshToken == null || user.RefreshTokenExpireTime == null)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.TokenInvalid,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.TokenInvalid)
                });
            }

            if (user.RefreshTokenExpireTime < DateTime.UtcNow || refreshToken != user.RefreshToken)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.TokenInvalid,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.TokenInvalid)
                });
            }

            //if (!await _userManager.VerifyUserTokenAsync(user, "RefreshTokenProvider", "RefreshToken", refreshToken))
            //{
            //    return BadRequest(new MessageWithCode
            //    {
            //        Code = (int)RespondCode.TokenInvalid,
            //        Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.TokenInvalid)
            //    });
            //}

            var newAuthTokenToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = await _tokenService.GenerateRefreshToken(user);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(30);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new TokenDto
                {
                    AuthToken = newAuthTokenToken,
                    RefreshToken = newRefreshToken
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


        [ServiceFilter(typeof(ValidationFilter))]
        [Authorize]
        [HttpPost("infochange")]
        public async Task<IActionResult> UserChangeInfoAsync(UserInfoChangeDto userInfoChangeDto)
        {
            var user = await _userManager.FindByIdAsync(userInfoChangeDto.UserID.ToString());
            var userIdFromToken = _userManager.GetUserId(HttpContext.User);

            if (userIdFromToken != user.Id.ToString())
            {
                return Unauthorized(new MessageWithCode
                {
                    Code = (int)RespondCode.TokenInvalid,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.TokenInvalid)
                });
            }

            if(user.UserName != userInfoChangeDto.UserName)
            {
                user.UserName = userInfoChangeDto.UserName;
            }

            if (user.UserNickName != userInfoChangeDto.UserNickName)
            {
                user.UserNickName = userInfoChangeDto.UserNickName;
            }

            var result = await _userManager.UpdateAsync(user);

            var userRev = await _repositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(2);
            userRev.Rev++;
            _repositoryWrapper.LangTextRevNumberRepo.Update(userRev);

            await _repositoryWrapper.LangTextRevNumberRepo.SaveAsync();

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    return BadRequest(error);
                }
            }

            return Ok(new MessageWithCode
            {
                Code = (int)RespondCode.Success,
                Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.Success)
            });
        }


        [ServiceFilter(typeof(ValidationFilter))]
        [Authorize]
        [HttpPost("passwordchange")]
        public async Task<IActionResult> UserPasswordChange(UserPasswordChangeDto userPasswordChangeDto)
        {
            var user = await _userManager.FindByIdAsync(userPasswordChangeDto.UserId.ToString());
            var userIdFromToken = _userManager.GetUserId(HttpContext.User);

            if (userIdFromToken != user.Id.ToString())
            {
                return Unauthorized(new MessageWithCode
                {
                    Code = (int)RespondCode.TokenInvalid,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.TokenInvalid)
                });
            }

            var checkCurrentPW = await _userManager.CheckPasswordAsync(user, userPasswordChangeDto.OldPassword);

            if (!checkCurrentPW)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.PasswordNotMatch,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.PasswordNotMatch)
                });
            }

            var result = await _userManager.ChangePasswordAsync(user, userPasswordChangeDto.OldPassword,
                userPasswordChangeDto.NewPasswordConfirm);

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

        [ServiceFilter(typeof(ValidationFilter))]
        [HttpPost("passwordrecovery")]
        public async Task<IActionResult> UserPasswordRecovery(UserPasswordRecoveryDto userPasswordRecoveryDto)
        {
            var user = await _userManager.FindByNameAsync(userPasswordRecoveryDto.UserName);

            if (user == null)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserNotFound,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserNotFound)
                });
            }

            var result = await _userManager.ResetPasswordAsync(user, userPasswordRecoveryDto.RecoveryCode, 
                userPasswordRecoveryDto.NewPasswordConfirm);

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

        [Authorize]
        [HttpGet("registrationcode")]
        public async Task<IActionResult> GetRegistrationCode()
        {
            var userIdFromToken = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userIdFromToken);

            if (user == null)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserNotFound,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserNotFound)
                });
            }

            var regCode = _tokenService.GenerateRegistrationCode();
            var isRegCodeExist = await _repositoryWrapper.RegistrationCodeRepo.IsExistAsync(regCode);


            if (!isRegCodeExist)
            {
                _repositoryWrapper.RegistrationCodeRepo.Create(new UserRegistrationCode
                {
                    Code = regCode,
                    RequestTimestamp = DateTime.UtcNow,
                    UserRequest = user.Id,
                    UserForRequest = user,
                });

                if(await _repositoryWrapper.RegistrationCodeRepo.SaveAsync())
                {
                    return Ok(regCode);
                }
                else
                {
                    return BadRequest(new MessageWithCode
                    {
                        Code = (int)RespondCode.GenerateRegistrationCodeFailed,
                        Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.GenerateRegistrationCodeFailed)
                    });
                }

            }
            else
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.GenerateRegistrationCodeFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.GenerateRegistrationCodeFailed)
                });
            }

        }

        [ServiceFilter(typeof(ValidationFilter))]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegistrationUserDto registrationUserDto)
        {

            var isExistUser = await _userManager.FindByNameAsync(registrationUserDto.UserName);

            if (isExistUser != null)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserNameExisted,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserNameExisted)
                });
            }

            var regCode = await _repositoryWrapper.RegistrationCodeRepo.GetByIdAsync(registrationUserDto.RegisterCode);

            if (regCode == null || regCode.UserUsed != null)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.RegistrationCodeInvalid,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.RegistrationCodeInvalid)
                });
            }

            var result = await _userManager.CreateAsync(new User
            {
                Id = Guid.NewGuid(),
                UserName = registrationUserDto.UserName,
                UserNickName = registrationUserDto.UserNickName,
                LockoutEnabled = false,
            }, registrationUserDto.ConfirmPassword);

            var userRev = await _repositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(2);
            userRev.Rev++;
            _repositoryWrapper.LangTextRevNumberRepo.Update(userRev);

            if (result.Succeeded)
            {
                var userAdded = await _userManager.FindByNameAsync(registrationUserDto.UserName);
                var addRoleResult = await _userManager.AddToRoleAsync(userAdded, "Editor");

                regCode.UsedTimestamp = DateTime.UtcNow;
                regCode.UserForUsed = userAdded;
                regCode.UserUsed = userAdded.Id;

                _repositoryWrapper.RegistrationCodeRepo.Update(regCode);

                await _repositoryWrapper.RegistrationCodeRepo.SaveAsync();
                await _repositoryWrapper.LangTextRevNumberRepo.SaveAsync();

                if (!addRoleResult.Succeeded)
                {
                    return Ok(new MessageWithCode
                    {
                        Code = (int)RespondCode.UserRoleSetFailed,
                        Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserRoleSetFailed)
                    });
                }
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
                    Code = (int)RespondCode.UserRegistrationFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.UserRegistrationFailed)
                });
            }
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<ActionResult<List<UserInClientDto>>> GetUsersAsync()
        {
            var users = await Task.Run(() => _userManager.Users.ToList());
            var userClientDto = _mapper.Map<List<UserInClientDto>>(users);

            return userClientDto;

        }

        [Authorize]
        [HttpGet("roles/{userGuid}")]
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

    }
}
