using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(AspUser user);
        Task<LoginResponse> RefreshToken(string token);
    }
}
