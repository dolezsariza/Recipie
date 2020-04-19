using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipie.Data;
using Recipie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Controllers
{
    [Route("tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly RecipeContext _context;

        public TagController(RecipeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTags()
        {
            var tags = await _context.Tags.ToListAsync();
            if (tags != null)
            {
                return Ok(tags);
            }

            return NotFound();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyTag(int id, [FromBody] Category modifiedTag)
        {
            if (UserAuthentication())
            {
                var tag = await _context.Tags.SingleOrDefaultAsync(cat => cat.Id == id);
                if (tag == null)
                {
                    return BadRequest();
                }

                tag.Name = modifiedTag.Name;

                try
                {
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return StatusCode(505);
                }
            }
            return BadRequest("You don't have permissions for this action!");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Category>> AddTag([FromBody] Category newTag)
        {
            var tag = new Tag(newTag.Name);

            _context.Tags.Add(tag);

            await _context.SaveChangesAsync();
            return Created("New tag created", "");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTag(int id)
        {
            if (UserAuthentication())
            {
                var tag = await _context.Tags.FindAsync(id);
                if (tag == null)
                {
                    return NotFound();
                }

                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();

                return Ok();
            }
            return BadRequest("You don't have permissions for this action!");
        }

        [HttpGet("{id}/recipes")]
        public async Task<ActionResult> GetRecipesInTag(int id)
        {
            var recipeIds = await _context.RecipeTags.Where(rt => rt.TagId == id).Select(rt => rt.RecipeId).ToListAsync();
            var recipes = await _context.Recipes.Where(rec => recipeIds.Contains(rec.ID)).ToListAsync();

            if (recipes == null)
            {
                return NotFound();
            }
            return Ok(recipes);
        }

        private bool UserAuthentication()
        {
            var currentUserName = User.Identity.Name;
            var user = _context.Users.Where(u => u.UserName == currentUserName).FirstOrDefault();

            if (user.RoleName == "admin" ) return true;
            return false;
        }
    }
}
