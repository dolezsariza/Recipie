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
using Recipie.Repositories.LoginRepository.Interfaces;

namespace StudyStud.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;

        public LoginController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginPostRequest login)
        {
            var user = await _loginRepository.Login(login);
            if (user == null)
            {
                return BadRequest("Wrong username or password");
            } 
            else
            { 
                 return Ok(new[] { user.Id, user.UserName, user.Email });
            }
        }
    }
}
