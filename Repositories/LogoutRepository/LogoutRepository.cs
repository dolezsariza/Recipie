using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipie.Models;
using Recipie.Repositories.LogoutRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.LogoutRepository
{
    public class LogoutRepository : ILogoutRepository
    {
        private readonly SignInManager<User> _signInManager;

        public LogoutRepository(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<bool> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
