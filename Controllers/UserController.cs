using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipie.Data;
using Recipie.Domain.Models;
using Recipie.Models;
using Recipie.Repositories.LoginRepository.Interfaces;
using Recipie.Repositories.UserRepository.Interfaces;

namespace Recipie.Controllers
{
    [Route("profile")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticator _authenticator;

        public UserController(IUserRepository userRepository, IAuthenticator authenticator)
        {
            _userRepository = userRepository;
            _authenticator = authenticator;
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetProfile(string userName)
        {
            var profile = await _userRepository.GetProfile(userName);
            if(profile != null) return Ok(profile);
            else return NotFound();
            
        }

        [Authorize]
        [HttpPut("{userName}")]
        public async Task<IActionResult> UpdateProfile(string userName, [FromBody]Profile profile)
        {
            if (!_authenticator.CheckIfUserIsOwnerOfProfile(userName,User.Identity.Name))
            {
                return BadRequest("You don't have permissions for this action!");
            }
            var result = await _userRepository.UpdateProfile(userName, profile);
            if (result) return Ok();
            else return NotFound();
        }

        [Authorize]
        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteProfile(string userName)
        {
            if (!_authenticator.CheckIfUserIsOwnerOfProfile(userName, User.Identity.Name))
            {
                return BadRequest("You don't have permissions for this action!");
            }
            var result = await _userRepository.DeleteProfile(userName);
            if (result) return NoContent();
            else return NotFound();
            
        }

        [Authorize]
        [HttpGet("{userName}/recipes")]
        public async Task<IActionResult> ListUsersRecipes(string userName)
        {
            var result = await _userRepository.ListUsersRecipes(userName);
            return Ok(result);
            
        }
    }
}
