using Microsoft.EntityFrameworkCore;
using Recipie.Data;
using Recipie.Domain.Models;
using Recipie.Models;
using Recipie.Repositories.RecipeRepository.Interfaces;
using Recipie.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.RecipeRepository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly RecipeContext _context;

        public RecipeRepository(RecipeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipes()
        {
            var recipes = await _context.Recipes.ToListAsync();

            if (recipes != null)
            {
                recipes.Sort((x, y) => x.Name.CompareTo(y.Name));
            }
            return recipes;
        }

        public async Task<Recipe> GetRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            return recipe;
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsOfRecipe(int id)
        {
            var ingredientIds = await _context.RecipeIngredients.Where(ri => ri.RecipeId == id).Select(ri => ri.IngredientId).ToListAsync();
            if (ingredientIds != null)
            {
                var ingredients = await _context.Ingredients.Where(i => ingredientIds.Contains(i.ID)).ToListAsync();
                return ingredients;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetTags(int id)
        {
            var tagIds = await _context.RecipeTags.Where(rt => rt.RecipeId == id).Select(rt => rt.TagId).ToListAsync();
            if (tagIds != null)
            {
                var tags = await _context.Tags.Where(t => tagIds.Contains(t.Id)).ToListAsync();
                return tags;
            }
            return null;

        }

        public async Task<bool> AddRecipe(RecipePostRequest recipeInfo)
        {
            try
            {
                ReplaceDotsInRecipeInfo(recipeInfo);
                var recipe = new Recipe(recipeInfo.Name, recipeInfo.Description, recipeInfo.OwnerId);
                recipe.CategoryId = int.Parse(recipeInfo.CategoryId);
                recipe.SubCategoryId = int.Parse(recipeInfo.SubCategoryId);
                recipe.OwnerName = recipeInfo.OwnerName;
                recipe.Date = recipeInfo.Date;
                recipe.Carbohydrate = float.Parse(recipeInfo.Carbohydrate);
                recipe.Energy = int.Parse(recipeInfo.Energy);
                recipe.Fat = float.Parse(recipeInfo.Fat);
                recipe.Protein = float.Parse(recipeInfo.Protein);
                recipe.Salt = float.Parse(recipeInfo.Salt);
                recipe.Sugar = float.Parse(recipeInfo.Sugar);

                _context.Recipes.Add(recipe);

                await _context.SaveChangesAsync();
                return true;
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public async Task<bool> ModifyRecipe(int id, RecipePostRequest modifiedRecipe)
        {
            try
            {
                var recipe = await _context.Recipes.SingleOrDefaultAsync(recipe => recipe.ID == id);
                if (recipe == null)
                {
                    return false;
                }
            
                ReplaceDotsInRecipeInfo(modifiedRecipe);

                recipe.Name = modifiedRecipe.Name;
                recipe.Description = modifiedRecipe.Description;
                recipe.Energy = int.Parse(modifiedRecipe.Energy);
                recipe.Fat = float.Parse(modifiedRecipe.Fat);
                recipe.Carbohydrate = float.Parse(modifiedRecipe.Carbohydrate);
                recipe.Sugar = float.Parse(modifiedRecipe.Sugar);
                recipe.Protein = float.Parse(modifiedRecipe.Protein);
                recipe.Salt = float.Parse(modifiedRecipe.Salt);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }
        public async Task<bool> DeleteRecipe(int id)
        {
            try
            {
                var recipe = await _context.Recipes.FindAsync(id);
                if (recipe == null)
                {
                    return false;
                }

                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public async Task<bool> AddTag(int id, int tagId)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            var tag = await _context.Tags.FindAsync(tagId);
            if (recipe == null || tag == null)
            {
                return false;
            }

            recipe.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return true;
        }

        private void ReplaceDotsInRecipeInfo(RecipePostRequest recipeInfo)
        {
            recipeInfo.Carbohydrate = recipeInfo.Carbohydrate.Replace(".", ",");
            recipeInfo.Fat = recipeInfo.Fat.Replace(".", ",");
            recipeInfo.Protein = recipeInfo.Protein.Replace(".", ",");
            recipeInfo.Salt = recipeInfo.Salt.Replace(".", ",");
            recipeInfo.Sugar = recipeInfo.Sugar.Replace(".", ",");
        }

    }
}
