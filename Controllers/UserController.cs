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

namespace Recipie.Controllers
{
    [Route("profile")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RecipeContext _context;

        public UserController(RecipeContext context)
        {
            _context = context;
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetProfile(string userName)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(user => user.UserName == userName);
                Profile profile = new Profile()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Introduction = user.Introduction,
                };
                return Ok(profile);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPut("{userName}")]
        public async Task<IActionResult> UpdateProfile(string userName, [FromBody]Profile profile)
        {
            if (!UserAuthentication(userName))
            {
                return BadRequest("You don't have permissions for this action!");
            }
            try
            {
                var nuser = await _context.Users.SingleOrDefaultAsync(user => user.UserName == userName);

                nuser.FirstName = profile.FirstName;
                nuser.LastName = profile.LastName;
                nuser.Introduction = profile.Introduction;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return NotFound();
            }

        }

        [Authorize]
        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteProfile(string userName)
        {
            if (!UserAuthentication(userName))
            {
                return Unauthorized("You don't have permissions for this action!");
            }
            try
            {
                User user = await _context.Users.SingleOrDefaultAsync(user => user.UserName == userName);
                List<Recipe> recipesOfUser = await _context.Recipes.Where(topic => topic.OwnerId == user.Id).ToListAsync();

                if (recipesOfUser != null)
                {
                    foreach (Recipe recipe in recipesOfUser)
                    {
                        recipe.OwnerId = null;
                    }
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest("User not found");
            }
        }

        [Authorize]
        [HttpGet("{userName}/recipes")]
        public async Task<IActionResult> ListUsersRecipes(string userName)
        {
            try
            {
                User user = await _context.Users.SingleOrDefaultAsync(user => user.UserName == userName);
                List<Recipe> recipesOfUser = await _context.Recipes.Where(rec => rec.OwnerId == user.Id).ToListAsync();

                return Ok(recipesOfUser);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        private bool UserAuthentication(string username)
        {
            var currentUserName = User.Identity.Name;
            if (currentUserName == username) return true;
            return false;
        }

    }
}
