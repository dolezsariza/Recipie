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
    [Route("ingredients")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly RecipeContext _context;

        public IngredientController(RecipeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllIngredients()
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            if (ingredients != null)
            {
                return Ok(ingredients);
            }

            return NotFound();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            return Ok(ingredient);
        }

        [HttpGet("{id}/recipes")]
        public async Task<ActionResult> GetRecipesFromIngredient(int id)
        {
            var recipeIds = await _context.RecipeIngredients.Where(ri => ri.IngredientId == id).Select(ri => ri.RecipeId).ToListAsync();
            if (recipeIds == null)
            {
                return NotFound();
            }

            var recipes = await _context.Recipes.Where(r => recipeIds.Contains(r.ID)).ToListAsync();
            if (recipes == null)
            {
                return NotFound();
            }

            return Ok(recipes);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyIngredient(int id, [FromBody]Ingredient modifiedIngredient)
        {
            var ingredient = await _context.Ingredients.SingleOrDefaultAsync(ingredient => ingredient.ID == id);
            if (ingredient == null)
            {
                return BadRequest();
            }

            ingredient.Name = modifiedIngredient.Name;
            ingredient.Description = modifiedIngredient.Description;
            ingredient.Energy = modifiedIngredient.Energy;
            ingredient.Fat = modifiedIngredient.Fat;
            ingredient.Carbohydrate = modifiedIngredient.Carbohydrate;
            ingredient.Sugar = modifiedIngredient.Sugar;
            ingredient.Protein = modifiedIngredient.Protein;
            ingredient.Salt = modifiedIngredient.Salt;

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
        public async Task<ActionResult<Ingredient>> AddIngredient([FromBody] IngredientPostRequest ingredientInfo)
        {
            var ingredient = new Ingredient(ingredientInfo.Name, ingredientInfo.Description);

            _context.Ingredients.Add(ingredient);

            await _context.SaveChangesAsync();
            return Created("New ingredient created", "");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
