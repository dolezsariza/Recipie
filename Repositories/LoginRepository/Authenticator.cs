using Recipie.Data;
using Recipie.Repositories.LoginRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.LoginRepository
{
    public class Authenticator : IAuthenticator
    {
        private RecipeContext _context;
        public Authenticator(RecipeContext context)
        {
            _context = context;
        }
        public bool AuthenticateUser(string userName)
        {
            var user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            if(user != null) return (user.RoleName == "admin");
            return false;
        }

        public bool CheckIfUserIsOwnerOfRecipe(string userName, int recipeId)
        {
            var recipe = _context.Recipes.Where(rec => rec.ID == recipeId).FirstOrDefault();
            return (recipe.OwnerName == userName);
        }
    }
}
