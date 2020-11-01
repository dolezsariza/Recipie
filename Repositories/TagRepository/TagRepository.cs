using Microsoft.EntityFrameworkCore;
using Recipie.Data;
using Recipie.Domain.Models;
using Recipie.Models;
using Recipie.Repositories.TagRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.TagRepository
{
    public class TagRepository : ITagRepository
    {
        private readonly RecipeContext _context;

        public TagRepository(RecipeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            var tags = await _context.Tags.ToListAsync();
            return tags;
        }

        public async Task<Tag> GetTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            return tag;
        }
        public async Task<IEnumerable<Recipe>> GetRecipesInTag(int id)
        {
            var recipeIds = await _context.RecipeTags.Where(rt => rt.TagId == id).Select(rt => rt.RecipeId).ToListAsync();
            var recipes = await _context.Recipes.Where(rec => recipeIds.Contains(rec.ID)).ToListAsync();
            return recipes;
        }

        public async Task<bool> AddTag(Tag newTag)
        {
            try
            {
                var tag = new Tag(newTag.Name);
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
                return true;
            } catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public async Task<bool> ModifyTag(int id, Tag modifiedTag)
        {
            try
            {
                var tag = await _context.Tags.SingleOrDefaultAsync(cat => cat.Id == id);
                if (tag == null)
                {
                    return false;
                }

                tag.Name = modifiedTag.Name;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
        public async Task<bool> DeleteTag(int id)
        {
            try
            {
                var tag = await _context.Tags.FindAsync(id);
                if (tag == null)
                {
                    return false;
                }

                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
                return true;
            } 
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
