using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs.Response;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<AspUser> userManager;

        public TokenService(
            IConfiguration configuration,
            UserManager<AspUser> userManager
            )
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }


        public string GenerateJwtToken(AspUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(configuration["JwtExpiryInDays"]));

            var token = new JwtSecurityToken(
                configuration["JwtIssuer"],
                configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            var tokenHanlder = new JwtSecurityTokenHandler();
            return tokenHanlder.WriteToken(token);
        }

        
        public async Task<LoginResponse> RefreshToken(string token)
        {
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]))
            };

            ClaimsPrincipal principal;
            try
            {
                principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParams, out var securityToken);
            }
            catch 
            {
                return null;
            }

            if (principal == null)
                return null;

            if (!principal.Identity.IsAuthenticated)
                return null;

            var userEmail = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

            if (userEmail == null)
                return null;

            var userDb = await userManager.FindByEmailAsync(userEmail);

            if (userDb == null)
                return null;

            return new LoginResponse
            {
                AspUserId = userDb.Id,
                Email = userDb.Email,
                Token = GenerateJwtToken(userDb)
            };
        }
    }
}
