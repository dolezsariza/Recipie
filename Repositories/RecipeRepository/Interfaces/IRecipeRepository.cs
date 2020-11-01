using Recipie.Domain.Models;
using Recipie.Models;
using Recipie.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.RecipeRepository.Interfaces
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetAllRecipes();
        Task<Recipe> GetRecipe(int id);
        Task<IEnumerable<Ingredient>> GetIngredientsOfRecipe(int id);
        Task<bool> ModifyRecipe(int id, RecipePostRequest recipeInfo);
        Task<bool> AddRecipe(RecipePostRequest recipeInfo);
        Task<bool> DeleteRecipe(int id);
        Task<IEnumerable<Tag>> GetTags(int id);
        Task<bool> AddTag(int id, int tagId);

    }
}
