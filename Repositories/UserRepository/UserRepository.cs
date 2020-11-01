using Microsoft.EntityFrameworkCore;
using Recipie.Data;
using Recipie.Domain.Models;
using Recipie.Models;
using Recipie.Repositories.UserRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly RecipeContext _context;
        public UserRepository(RecipeContext context)
        {
            _context = context;
        }

        public async Task<Profile> GetProfile(string username)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(user => user.UserName == username);
                var profile = new Profile()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Introduction = user.Introduction,
                };
                return profile;
            } 
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<bool> UpdateProfile(string username, Profile profile)
        { 
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);

                user.FirstName = profile.FirstName;
                user.LastName = profile.LastName;
                user.Introduction = profile.Introduction;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }
        public async Task<bool> DeleteProfile(string username)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(user => user.UserName == username);
                var recipesOfUser = await _context.Recipes.Where(topic => topic.OwnerId == user.Id).ToListAsync();

                if (recipesOfUser != null)
                {
                    foreach (Recipe recipe in recipesOfUser)
                    {
                        recipe.OwnerId = null;
                    }
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }
        public async Task<IEnumerable<Recipe>> ListUsersRecipes(string username)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(user => user.UserName == username);
                var recipesOfUser = await _context.Recipes.Where(rec => rec.OwnerId == user.Id).ToListAsync();

                return recipesOfUser;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}
