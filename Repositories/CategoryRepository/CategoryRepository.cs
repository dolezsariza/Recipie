using Microsoft.EntityFrameworkCore;
using Recipie.Data;
using Recipie.Domain.Models;
using Recipie.Models;
using Recipie.Repositories.CategoryRepository.Interfaces;
using Recipie.Repositories.LoginRepository;
using Recipie.Repositories.LoginRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly RecipeContext _context;

        public CategoryRepository(RecipeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category;
        }

        public async Task<IEnumerable<Recipe>> GetRecipesOfCategory(int id)
        {
            var recipes = await _context.Recipes.Where(rec => rec.CategoryId == id).ToListAsync();
            return recipes;
        }

        public async Task<bool> AddCategory(string name, string description)
        {
            try
            {
                var category = new Category(name, description);
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> ModifyCategory(int id, string name, string description)
        {
            try
            {
                var category = await _context.Categories.SingleOrDefaultAsync(cat => cat.Id == id);
                category.Name = name;
                category.Description = description;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Category not found");
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

        }

        public async Task<bool> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                var subcategories = _context.SubCategories.Where(sub => sub.CategoryId == id);
                foreach (var sub in subcategories)
                {
                    await ModifySubCategory(0, sub.Id, sub.Name, sub.Description);
                }
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            } 
            catch (NullReferenceException)
            {
                Console.WriteLine("Category not found");
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        //SubCategory

        public async Task<IEnumerable<SubCategory>> GetSubCategories(int id)
        {
            var subcategories = await _context.SubCategories.Where(sub => sub.CategoryId == id).ToListAsync();
            return subcategories;
        }

        public async Task<SubCategory> GetSubCategory(int id, int subId)
        {
            var subcategory = await _context.SubCategories.FindAsync(subId);
            return subcategory;
        }

        public async Task<IEnumerable<Recipe>> GetRecipesOfSubCategory(int id, int subId)
        {
            var recipes = await _context.Recipes.Where(rec => rec.SubCategoryId == subId && rec.CategoryId == id).ToListAsync();
            return recipes;
        }

        public async Task<bool> AddSubCategory(int categoryId, string name, string description)
        {
            try
            {
                var subcategory = new SubCategory(name, description, categoryId);

                _context.SubCategories.Add(subcategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> ModifySubCategory(int id, int subId, string name, string description)
        {
            try
            {
                var subcategory = await _context.SubCategories.SingleOrDefaultAsync(sub => sub.Id == id);
                subcategory.Name = name;
                subcategory.Description = description;
                subcategory.CategoryId = id;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Subcategory not found");
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSubCategory(int id, int subId)
        {
            try
            {
                var subCategory = await _context.SubCategories.FindAsync(id);
                _context.SubCategories.Remove(subCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Subcategory not found");
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}
