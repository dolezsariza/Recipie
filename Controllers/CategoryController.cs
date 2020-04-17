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
    [Route("categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly RecipeContext _context;

        public CategoryController(RecipeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories != null)
            {
                return Ok(categories);
            }

            return NotFound();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyCategory(int id, [FromBody]Category modifiedCategory)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(cat => cat.Id == id);
            if (category == null)
            {
                return BadRequest();
            }

            category.Name = modifiedCategory.Name;
            category.Description = modifiedCategory.Description;

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

        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory([FromBody] Category newCategory)
        {
            var category = new Category(newCategory.Name, newCategory.Description);

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();
            return Created("New category created", "");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //Subcategory

        [HttpGet("{id}/subcategories")]
        public async Task<ActionResult> GetSubsOfCategory(int id)
        {
            var subcategories = await _context.SubCategories.Where(sub => sub.CategoryId == id).ToListAsync();
            if (subcategories == null)
            {
                return NotFound();
            }

            return Ok(subcategories);
        }

        [HttpGet("{id}/subcategories/{subId}")]
        public async Task<ActionResult> GetSubCategory(int id, int subId)
        {
            var subcategory = await _context.SubCategories.FindAsync(subId);

            if (subcategory == null)
            {
                return NotFound();
            }

            return Ok(subcategory);
        }

        [HttpPut("{id}/subcategories/{subId}")]
        public async Task<IActionResult> ModifySubCategory(int id, [FromBody]SubCategory modifiedSubCategory)
        {
            var subcategory = await _context.SubCategories.SingleOrDefaultAsync(sub => sub.Id == id);
            if (subcategory == null)
            {
                return BadRequest();
            }

            subcategory.Name = modifiedSubCategory.Name;
            subcategory.Description = modifiedSubCategory.Description;

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

        [HttpPost("{id}/subcategories")]
        public async Task<ActionResult<Category>> AddSubCategory(int id, [FromBody] SubCategory newSubCategory)
        {
            var subcategory = new SubCategory(newSubCategory.Name, newSubCategory.Description, id);

            _context.SubCategories.Add(subcategory);

            await _context.SaveChangesAsync();
            return Created("New subcategory created", "");
        }

        [HttpDelete("{id}/subcategories/{subId}")]
        public async Task<ActionResult> DeleteSubCategory(int id, int subId)
        {
            var subcategory = await _context.SubCategories.FindAsync(subId);
            if (subcategory == null)
            {
                return NotFound();
            }

            _context.SubCategories.Remove(subcategory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Get recipes by category

        [HttpGet("{id}/recipes")]
        public async Task<ActionResult> GetRecipesOfCategory(int id)
        {
            var recipes = await _context.Recipes.Where(rec => rec.CategoryId == id).ToListAsync();
            if (recipes == null)
            {
                return NotFound();
            }
            return Ok(recipes);
        }

        [HttpGet("{id}/subcategories/{subId}/recipes")]
        public async Task<ActionResult> GetARecipeOfCategory(int id, int subId)
        {
            var recipes = await _context.Recipes.Where(rec => rec.SubCategoryId == subId).ToListAsync();
            if (recipes == null)
            {
                return NotFound();
            }
            return Ok(recipes);
        }
    }
}
