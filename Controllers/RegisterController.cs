using System;
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
using Recipie.Repositories.RegisterRepository.Interfaces;
using Recipie.RequestModels;

namespace Recipie.Controllers
{
    [Route("/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterRepository _registerRepository;
        public RegisterController(IRegisterRepository registerRepository)
        {
            _registerRepository = registerRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterPostRequest registerRequest)
        {
            var user = await _registerRepository.Register(registerRequest);

            if (user != null)
            {
                return Created("", user);
            }
            return BadRequest();
        }
    }
}
