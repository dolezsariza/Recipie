using Microsoft.AspNetCore.Identity;
using Recipie.Models;
using Recipie.Repositories.RegisterRepository.Interfaces;
using Recipie.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Recipie.Repositories.RegisterRepository
{
    public class RegisterRepository : IRegisterRepository
    {

        private readonly UserManager<User> _userManager;
        public RegisterRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<User> Register(RegisterPostRequest registerRequest)
        {
            try
            {
                var user = new User { UserName = registerRequest.Username, Email = registerRequest.Email };
                user.RoleName = "user";

                var result = await _userManager.CreateAsync(user, registerRequest.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
                    return user;
                } else
                {
                    Console.WriteLine(result.Errors.Select(x => x.Description));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;

        }
    }
}
