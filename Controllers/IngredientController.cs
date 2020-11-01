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
using Recipie.Repositories.IngredientRepository.Interfaces;
using Recipie.Repositories.LoginRepository.Interfaces;

namespace Recipie.Controllers
{
    [Route("ingredients")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IAuthenticator _authenticator;

        public IngredientController(IIngredientRepository ingredientRepository, IAuthenticator authenticator)
        {
            _ingredientRepository = ingredientRepository;
            _authenticator = authenticator;

        }

        [HttpGet]
        public async Task<ActionResult> GetAllIngredients()
        {
            var ingredients = await _ingredientRepository.GetAllIngredients();
            if (ingredients != null)
            {
                return Ok(ingredients);
            }

            return NotFound();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetIngredient(int id)
        {
            var ingredient = await _ingredientRepository.GetIngredient(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return Ok(ingredient);
        }

        [HttpGet("{id}/recipes")]
        public async Task<ActionResult> GetRecipesFromIngredient(int id)
        {
            var recipes = await _ingredientRepository.GetRecipesFromIngredient(id);
            if (recipes == null)
            {
                return NotFound();
            }
            return Ok(recipes);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyIngredient(int id, [FromBody]Ingredient modifiedIngredient)
        {
            if (_authenticator.AuthenticateUser(User.Identity.Name))
            {
                var result = await _ingredientRepository.ModifyIngredient(id, modifiedIngredient);
                if (result) return Ok();
                else return BadRequest("Modification unsuccessful");
            }
            return BadRequest("You don't have permissions for this action!");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Ingredient>> AddIngredient([FromBody] IngredientPostRequest ingredientInfo)
        {
            var result = await _ingredientRepository.AddIngredient(ingredientInfo);

            if (result) return Created("New ingredient created", "");
            else return BadRequest("Addition unsuccessful");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIngredient(int id)
        {
            if (_authenticator.AuthenticateUser(User.Identity.Name))
            {
                var result = await _ingredientRepository.DeleteIngredient(id);
                if (result) return Ok();
                else return BadRequest("Deletion unsuccessful");
            }
            return BadRequest("You don't have permissions for this action!");
        }
    }
}
