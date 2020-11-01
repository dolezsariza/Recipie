using Recipie.Domain.Models;
using Recipie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.CategoryRepository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategory(int id);
        Task<bool> AddCategory(string name, string description);
        Task<bool> ModifyCategory(int id, string name, string description);
        Task<bool> DeleteCategory(int id);
        Task<IEnumerable<SubCategory>> GetSubCategories(int id);
        Task<SubCategory> GetSubCategory(int id, int subId);
        Task<bool> AddSubCategory(int id, string name, string description);
        Task<bool> ModifySubCategory(int id, int subId, string name, string description);
        Task<bool> DeleteSubCategory(int id, int subId);
        Task<IEnumerable<Recipe>> GetRecipesOfCategory(int id);
        Task<IEnumerable<Recipe>> GetRecipesOfSubCategory(int id, int subId);

    }
}
