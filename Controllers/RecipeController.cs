using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipie.Models;
using Recipie.RequestModels;
using Recipie.Data;
using Recipie.Domain.Models;

namespace Recipie.Controllers
{
    [Route("recipes")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeContext _context;

        public RecipeController(RecipeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRecipes()
        {
            var recipes = await _context.Recipes.ToListAsync();
            if (recipes != null)
            {
                return Ok(recipes);
            }

            return NotFound();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [HttpGet("{id}/ingredients")]
        public async Task<ActionResult> GetIngredientsOfRecipe(int id)
        {
            var ingredientIds = await _context.RecipeIngredients.Where(ri => ri.RecipeId == id).Select(ri => ri.IngredientId).ToListAsync();
            if (ingredientIds == null)
            {
                return NotFound();
            }
            
            var ingredients = await _context.Ingredients.Where(i => ingredientIds.Contains(i.ID)).ToListAsync();
            if (ingredients == null)
            {
                return NotFound();
            }

            return Ok(ingredients);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyRecipe(int id, [FromBody]Recipe modifiedRecipe)
        {
            var recipe = await _context.Recipes.SingleOrDefaultAsync(recipe => recipe.ID == id);
            if (recipe == null)
            {
                return BadRequest();
            }

            recipe.Name = modifiedRecipe.Name;
            recipe.Description = modifiedRecipe.Description;
            recipe.Energy = modifiedRecipe.Energy;
            recipe.Fat = modifiedRecipe.Fat;
            recipe.Carbohydrate = modifiedRecipe.Carbohydrate;
            recipe.Sugar = modifiedRecipe.Sugar;
            recipe.Protein = modifiedRecipe.Protein;
            recipe.Salt = modifiedRecipe.Salt;


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
        public async Task<ActionResult<Recipe>> AddRecipe([FromBody] RecipePostRequest recipeInfo)
        {
            var recipe = new Recipe(recipeInfo.Name, recipeInfo.Description);
            recipe.Name = recipeInfo.Name;
            recipe.Description = recipeInfo.Description;
        
            _context.Recipes.Add(recipe);

            await _context.SaveChangesAsync();
            return Created("New recipe created", "");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
