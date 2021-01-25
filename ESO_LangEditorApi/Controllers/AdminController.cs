using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Controllers
{
    [Route("api/admin/user")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        public IMapper Mapper { get; }

        public AdminController(RoleManager<Role> roleManager, UserManager<User> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            Mapper = mapper;

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> UserList()
        {
            var users = _userManager.Users.ToList();

            var usersDto = Mapper.Map<List<UserDto>>(users);

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

            var userDto = Mapper.Map<UserDto>(user);

            return userDto;
        }


        //[HttpGet("{userGuid}")]
        public ActionResult<UserDto> UserChangeProfileBySelf(Guid userGuid, UserProfileModifyBySelfDto userProfileModifyBySelf)
        {
            var user = Mapper.Map<User>(userProfileModifyBySelf);

            user.Id = userGuid;

            if (user == null)
            {
                return NotFound();
            }

            var userDto = Mapper.Map<UserDto>(user);

            return userDto;
        }


        [HttpPost("addrole/{roleName}", Name = nameof(AddRoleAsync))]
        public async Task<ActionResult> AddRoleAsync(string roleName)
        {
            bool isRoleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!isRoleExist)
            {
                await _roleManager.CreateAsync(new Role { Name = roleName });

                return Ok();
            }

            return BadRequest();
        }

        private async Task<ActionResult> AddUserToRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                return NotFound();
            }

            bool isRoleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!isRoleExist)
            {
                //await _roleManager.CreateAsync(new Role { Name = roleName });

                return NotFound();
            }
            else
            {
                await _userManager.IsInRoleAsync(user, roleName);
                return Ok();

            }

            //await _userManager.AddToRoleAsync(user, roleName);

        }

    }
}
