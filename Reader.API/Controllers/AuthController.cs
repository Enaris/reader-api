using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response;
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
        private readonly IConfiguration configuration;

        public AuthController(UserManager<AspUser> userManager, 
            SignInManager<AspUser> signInManager, 
            ITokenService tokenService, 
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new AspUser { Email = request.Email, UserName = request.Email };
            var created = await userManager.CreateAsync(user, request.Password);

            if (!created.Succeeded)
                return BadRequest(created.Errors);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var userDb = await userManager.FindByEmailAsync(request.Email);

            if (userDb == null)
                return BadRequest(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("Login", "Email or password is invalid") });

            var loginResult = await signInManager.CheckPasswordSignInAsync(userDb, request.Password, false);

            if (!loginResult.Succeeded)
                return BadRequest(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("Login", "Email or password is invalid") });

            var token = tokenService.GenerateJwtToken(userDb);

            return Ok(new LoginResponse {
                AspUserId = userDb.Id, 
                Email = request.Email, 
                Token = token
            });
        }

        [HttpPost("checkToken/{token}")]
        public async Task<IActionResult> RefreshToken([FromRoute] string token)
        {
            var result = await tokenService.RefreshToken(token);

            if (result == null)
                return BadRequest(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("token", "Token was invalid") });

            return Ok(result);
        }

    }
}