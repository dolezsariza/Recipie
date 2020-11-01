using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipie.Data;
using Recipie.Models;
using Recipie.Repositories.CategoryRepository.Interfaces;
using Recipie.Repositories.LoginRepository.Interfaces;
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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthenticator _authenticator;

        public CategoryController(ICategoryRepository categoryRepository, IAuthenticator authenticator)
        {
            _categoryRepository = categoryRepository;
            _authenticator = authenticator;            
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            if (categories != null)
            {
                return Ok(categories);
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategory(int id)
        {
            var category = await _categoryRepository.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory([FromBody] Category newCategory)
        {
            if (_authenticator.AuthenticateUser(User.Identity.Name))
            {
                var category = await _categoryRepository.AddCategory(newCategory.Name, newCategory.Description);
                if(category) return Created("New category created", "");
                return BadRequest("Addition unsuccessful");
            }
            return BadRequest("You don't have permissions for this action!");
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyCategory(int id, [FromBody]Category modifiedCategory)
        {
            if (_authenticator.AuthenticateUser(User.Identity.Name))
            {
                var category = await _categoryRepository.ModifyCategory(id, modifiedCategory.Name, modifiedCategory.Description);
                if (category)
                {
                    return Ok();
                }
                return NotFound();               
            }
            return BadRequest("You don't have permissions for this action!");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            if (_authenticator.AuthenticateUser(User.Identity.Name))
            {
                var category = await _categoryRepository.DeleteCategory(id);
                if (category) return Ok();
                return BadRequest("Deletion unsuccessful");
            }
            return BadRequest("You don't have permissions for this action!");
        }

        [HttpGet("{id}/recipes")]
        public async Task<ActionResult> GetRecipesOfCategory(int id)
        {
            var recipes = await _categoryRepository.GetRecipesOfCategory(id);
            if (recipes == null)
            {
                return NotFound();
            }
            return Ok(recipes);
        }

        //Subcategory

        [HttpGet("{id}/subcategories")]
        public async Task<ActionResult> GetSubsOfCategory(int id)
        {
            var subcategories = await _categoryRepository.GetSubCategories(id);
            if (subcategories == null)
            {
                return NotFound();
            }

            return Ok(subcategories);
        }

        [HttpGet("{id}/subcategories/{subId}")]
        public async Task<ActionResult> GetSubCategory(int id, int subId)
        {
            var subcategory = await _categoryRepository.GetSubCategory(id, subId);

            if (subcategory == null)
            {
                return NotFound();
            }

            return Ok(subcategory);
        }

        [Authorize]
        [HttpPost("{id}/subcategories")]
        public async Task<ActionResult<Category>> AddSubCategory(int id, [FromBody] SubCategory newSubCategory)
        {
            if (_authenticator.AuthenticateUser(User.Identity.Name))
            {
                var subcategory = await _categoryRepository.AddSubCategory(id, newSubCategory.Name, newSubCategory.Description);
                if(subcategory) return Created("New subcategory created", "");
                return BadRequest("Addition unsuccessful");
            }
            return BadRequest("You don't have permissions for this action!");
        }

        [Authorize]
        [HttpPut("{id}/subcategories/{subId}")]
        public async Task<IActionResult> ModifySubCategory(int id, [FromBody]SubCategory modifiedSubCategory)
        {
            if (_authenticator.AuthenticateUser(User.Identity.Name))
            {
                var subcategory = await _categoryRepository.ModifySubCategory(id, modifiedSubCategory.Id, modifiedSubCategory.Name, modifiedSubCategory.Description);
                if(subcategory) return Ok();
                return BadRequest("Modification unsuccessful");
                
            }
            return BadRequest("You don't have permissions for this action!");
        }


        [Authorize]
        [HttpDelete("{id}/subcategories/{subId}")]
        public async Task<ActionResult> DeleteSubCategory(int id, int subId)
        {
            if (_authenticator.AuthenticateUser(User.Identity.Name))
            {
                var subcategory = await _categoryRepository.DeleteSubCategory(id, subId);
                if (subcategory) return Ok();
                return BadRequest("Deletion unsuccessful"); 
            }
            return BadRequest("You don't have permissions for this action!");
        }

        [HttpGet("{id}/subcategories/{subId}/recipes")]
        public async Task<ActionResult> GetRecipesOfSubcategory(int id, int subId)
        {
            var recipes = await _categoryRepository.GetRecipesOfSubCategory(id, subId);
            if (recipes == null)
            {
                return NotFound();
            }
            return Ok(recipes);
        }
    }
}
