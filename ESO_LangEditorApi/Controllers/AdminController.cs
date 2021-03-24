using AutoMapper;
using ESO_LangEditor.API.Services;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AdminController(RoleManager<Role> roleManager, UserManager<User> userManager, 
            IMapper mapper, ITokenService tokenService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> UserList()
        {
            var users = _userManager.Users.ToList();

            var usersDto = _mapper.Map<List<UserDto>>(users);

            return usersDto;
        }

        [HttpGet("{userGuid}")]
        public async Task<ActionResult<UserDto>> GetUser(string userGuid)
        {
            var user = await _userManager.FindByIdAsync(userGuid);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        [HttpPost("{userGuid}/roles")]
        public async Task<ActionResult<bool>> ModifyUserRolesAsync(string userGuid, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userGuid);
            var userIdFromToken = _userManager.GetUserId(HttpContext.User);

            if (user == null)
            {
                return BadRequest();
            }

            var roleForAdd = roles;

            foreach (var role in roles)
            {
                if (await _userManager.IsInRoleAsync(user, role))
                {
                    roleForAdd.Remove(role);
                }
            }

            if (roleForAdd.Count == 0)
            {
                return BadRequest();
            }

            var result = await _userManager.AddToRolesAsync(user, roleForAdd);

            return result.Succeeded;

        }

        [HttpPost("role")]
        public async Task<ActionResult<bool>> AddUserRoleAsync(string role)
        {
            //var user = await _userManager.FindByIdAsync(userGuid);
            var userIdFromToken = _userManager.GetUserId(HttpContext.User);

            if (await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest();
            }


            var result = await _roleManager.CreateAsync(new Role { Name = role });

            return result.Succeeded;

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> AddUserAsync(UserDto username)
        {
            Guid userId;

            if (username.ID == null || username.ID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                userId = new Guid();
            }
            else
            {
                userId = username.ID;
            }

            var user = new User
            {
                Id = userId,
                UserName = username.UserName,
                //Email = "123@2233.com",

            };

            var result = await _userManager.CreateAsync(user);

            foreach(var error in result.Errors)
            {
                Debug.WriteLine(error.Description);
            }
            

            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(user, "InitUser");

                var refreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpireTime = DateTime.Now.AddDays(7);

                await _userManager.UpdateAsync(user);
                var userDto = _mapper.Map<UserDto>(user);

                return userDto;

            }
            else
            {
                ModelState.AddModelError("Error", result.Errors.FirstOrDefault()?.Description);
                return BadRequest(ModelState);
            }

        }


    }
}
