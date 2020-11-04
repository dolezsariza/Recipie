using Microsoft.AspNetCore.Identity;
using Recipie.Data;
using Recipie.Models;
using Recipie.Repositories.LoginRepository.Interfaces;
using Recipie.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.LoginRepository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly RecipeContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginRepository(RecipeContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        public async Task<User> Login(LoginPostRequest login)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(login.Username);
                var result = await _userManager.CheckPasswordAsync(
                    user, login.Password);
                if(result)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return user;

                }
                return null;
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
