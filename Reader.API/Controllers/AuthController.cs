using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.Services;

namespace Reader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AspUser> userManager;
        private readonly SignInManager<AspUser> signInManager;
        private readonly ITokenService tokenService;

        public AuthController(UserManager<AspUser> userManager, 
            SignInManager<AspUser> signInManager, 
            ITokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new AspUser { Email = request.Email, UserName = request.Email };
            var created = await userManager.CreateAsync(user, request.Password);

            if (!created.Succeeded)
                return BadRequest();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var userDb = await userManager.FindByEmailAsync(request.Email);

            if (userDb == null)
                return BadRequest();

            var loginResult = await signInManager.CheckPasswordSignInAsync(userDb, request.Password, false);

            if (!loginResult.Succeeded)
                return BadRequest();


            var token = tokenService.GenerateJwtToken(userDb);

            return Ok(token);
        }
    }
}