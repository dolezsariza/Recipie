using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipie.Models;
using Recipie.Repositories.LogoutRepository.Interfaces;

namespace StudyStud.Controllers
{
    [Route("logout")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly ILogoutRepository _logoutRepository;

        public LogoutController(ILogoutRepository logoutRepository)
        {
            _logoutRepository = logoutRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var loggedOut = await _logoutRepository.Logout();
            if (loggedOut)
            {
                return Ok();
            }
            return StatusCode(500);
        }
    }
}