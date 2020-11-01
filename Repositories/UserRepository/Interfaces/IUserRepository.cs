using Recipie.Domain.Models;
using Recipie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.UserRepository.Interfaces
{
    public interface IUserRepository
    {
        Task<Profile> GetProfile(string username);
        Task<bool> UpdateProfile(string username, Profile profile);
        Task<bool> DeleteProfile(string username);
        Task<IEnumerable<Recipe>> ListUsersRecipes(string username);
    }
}
