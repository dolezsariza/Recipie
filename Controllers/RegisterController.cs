﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipie.Data;
using Recipie.Models;
using Recipie.RequestModels;

namespace Recipie.Controllers
{
    [Route("/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly RecipeContext _context;

        private readonly UserManager<User> _userManager;
        public RegisterController(UserManager<User> userManager, RecipeContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterPostRequest registerRequest)
        {
            var user = new User { UserName = registerRequest.Username, Email = registerRequest.Email };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);



            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
                return Created("", user);
            }

            return StatusCode(406, string.Join("\n", result.Errors.Select(x => x.Description)));
        }
    }
}
