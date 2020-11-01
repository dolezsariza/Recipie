using Recipie.Domain.Models;
using Recipie.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.IngredientRepository.Interfaces
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> GetAllIngredients();
        Task<Ingredient> GetIngredient(int id);
        Task<IEnumerable<Recipe>> GetRecipesFromIngredient(int id);
        Task<bool> AddIngredient(IngredientPostRequest newIngredient);
        Task<bool> ModifyIngredient(int id, Ingredient modifiedIngredient);
        Task<bool> DeleteIngredient(int id);
    }
}
