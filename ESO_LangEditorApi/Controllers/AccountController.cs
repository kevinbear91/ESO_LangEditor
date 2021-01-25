using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {

        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager,
          SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

                //await AddUserToRoleAsync(user, "Editor");
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

            var result = await _signInManager.PasswordSignInAsync(
                    loginUser.UserName, loginUser.Password, true, false);

            if (result.Succeeded)
            {

                //return RedirectToAction("index"，"home");
                return Ok();
            }



            //ModelState.AddModelError(string.Empty, "登录失败，请重试");
            return BadRequest();


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
