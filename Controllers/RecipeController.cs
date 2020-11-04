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
using Microsoft.AspNetCore.Authorization;
using Recipie.Repositories.RecipeRepository.Interfaces;
using Recipie.Repositories.LoginRepository.Interfaces;

namespace Recipie.Controllers
{
    [Route("recipes")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IAuthenticator _authenticator;

        public RecipeController(IRecipeRepository recipeRepository, IAuthenticator authenticator)
        {
            _authenticator = authenticator;
            _recipeRepository = recipeRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRecipes()
        {
            var recipes = await _recipeRepository.GetAllRecipes();

            if (recipes != null)
            {
                return Ok(recipes);
            }

            return NotFound();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetRecipe(int id)
        {
            var recipe = await _recipeRepository.GetRecipe(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [HttpGet("{id}/ingredients")]
        public async Task<ActionResult> GetIngredientsOfRecipe(int id)
        {
            var ingredients = await _recipeRepository.GetIngredientsOfRecipe(id);
            if (ingredients == null) return NotFound();
            return Ok(ingredients);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyRecipe(int id, [FromBody]RecipePostRequest modifiedRecipe)
        {
            if (!_authenticator.CheckIfUserIsOwnerOfRecipe(User.Identity.Name, id))
            {
                return Unauthorized("You don't have permissions for this action!");
            }

            var result = await _recipeRepository.ModifyRecipe(id, modifiedRecipe);
            if (!result)
            {
                return BadRequest("Modification unsusccessful");
            }
            
            return Ok();
            
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Recipe>> AddRecipe([FromBody] RecipePostRequest recipeInfo)
        {
            var result = await _recipeRepository.AddRecipe(recipeInfo);
            if (result) return Created("New recipe added", "");
            return BadRequest("Addition unsuccessful");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecipe(int id)
        {
            if (!_authenticator.CheckIfUserIsOwnerOfRecipe(User.Identity.Name, id))
            {
                return Unauthorized("You don't have permissions for this action!");
            }

            var result = await _recipeRepository.DeleteRecipe(id);
            if (result) return Ok();
            return BadRequest("Deletion unsuccessful");
        }

        [HttpGet("{id}/tags")]
        public async Task<ActionResult> GetTags(int id)
        {
            var tags = await _recipeRepository.GetTags(id);
            if(tags == null)
            {
                return NotFound();
            }
            return Ok(tags);
        }

        [Authorize]
        [HttpPost("{id}/addtag/{tagId}")]
        public async Task<ActionResult> AddTag(int id, int tagId)
        {
            if (!_authenticator.CheckIfUserIsOwnerOfRecipe(User.Identity.Name, id))
            {
                return Unauthorized("You don't have permissions for this action!");
            }

            var result = await _recipeRepository.AddTag(id, tagId);
            if(result) return Created("New tag added", "");
            return BadRequest("Tag couldn't be added");
        }
    }
}
