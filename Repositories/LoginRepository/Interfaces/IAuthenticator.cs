using Recipie.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.LoginRepository.Interfaces
{
    public interface IAuthenticator
    {
        bool AuthenticateUser(string userName);
        bool CheckIfUserIsOwnerOfRecipe(string userName, int recipeId);
    }
}
