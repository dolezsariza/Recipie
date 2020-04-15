using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipie.Models;
using Recipie.RequestModels;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;
using Recipie.Data;

namespace StudyStud.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly RecipeContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginController(RecipeContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginPostRequest login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user == null)
            {
                return BadRequest("Wrong username or password");
            }
            var result = await _userManager.CheckPasswordAsync(
                user, login.Password);

            if (result)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return Ok(new[] { user.Id, user.UserName, user.Email });
            }
            return BadRequest("Wrong username or password");
        }
    }
}
