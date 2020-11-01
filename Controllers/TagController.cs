using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipie.Data;
using Recipie.Models;
using Recipie.Repositories.LoginRepository.Interfaces;
using Recipie.Repositories.TagRepository.Interfaces;
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
        private readonly ITagRepository _tagRepository;
        private readonly IAuthenticator _authenticator;

        public TagController(ITagRepository tagRepository, IAuthenticator authenticator)
        {
            _tagRepository = tagRepository;
            _authenticator = authenticator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTags()
        {
            var tags = await _tagRepository.GetAllTags();
            if (tags != null)
            {
                return Ok(tags);
            }

            return NotFound();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetTag(int id)
        {
            var tag = await _tagRepository.GetTag(id);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyTag(int id, [FromBody] Tag modifiedTag)
        {
            if (_authenticator.AuthenticateUser(User.Identity.Name))
            {
                var result = await _tagRepository.ModifyTag(id, modifiedTag);
                if (result) return Ok();
                else return NotFound();
            }
            return BadRequest("You don't have permissions for this action!");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Tag>> AddTag([FromBody] Tag newTag)
        {
            var result = await _tagRepository.AddTag(newTag);
            if (result) return Created("New tag created", "");
            return BadRequest("Addition unsuccessful");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTag(int id)
        {
            if (_authenticator.AuthenticateUser(User.Identity.Name))
            {
                var result = await _tagRepository.DeleteTag(id);
                if(result) return Ok();
                return BadRequest("Deletion unsuccessful");
            }
            return BadRequest("You don't have permissions for this action!");
        }

        [HttpGet("{id}/recipes")]
        public async Task<ActionResult> GetRecipesInTag(int id)
        {
            var recipes = await _tagRepository.GetRecipesInTag(id);
            if (recipes == null)
            {
                return NotFound();
            }
            return Ok(recipes);
        }
    }
}
