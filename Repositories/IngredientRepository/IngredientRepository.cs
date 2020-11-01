using Microsoft.EntityFrameworkCore;
using Recipie.Data;
using Recipie.Domain.Models;
using Recipie.Repositories.IngredientRepository.Interfaces;
using Recipie.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.IngredientRepository
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly RecipeContext _context;

        public IngredientRepository(RecipeContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Ingredient>> GetAllIngredients()
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            return ingredients;
        }

        public async Task<Ingredient> GetIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            return ingredient;
        }

        public async Task<IEnumerable<Recipe>> GetRecipesFromIngredient(int id)
        {
            var recipeIds = await _context.RecipeIngredients.Where(ri => ri.IngredientId == id).Select(ri => ri.RecipeId).ToListAsync();
            if (recipeIds == null)
            {
                return null;
            }

            var recipes = await _context.Recipes.Where(r => recipeIds.Contains(r.ID)).ToListAsync();
            return recipes;
        }

        public async Task<bool> AddIngredient(IngredientPostRequest newIngredient)
        {
            try
            {
                var ingredient = new Ingredient(newIngredient.Name, newIngredient.Description);

                _context.Ingredients.Add(ingredient);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> DeleteIngredient(int id)
        {
            try
            {
                var ingredient = await _context.Ingredients.FindAsync(id);
                if (ingredient == null)
                {
                    return false;
                }

                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> ModifyIngredient(int id, Ingredient modifiedIngredient)
        {
            var ingredient = await _context.Ingredients.SingleOrDefaultAsync(ingredient => ingredient.ID == id);
            if (ingredient == null)
            {
                return false;
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
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}
