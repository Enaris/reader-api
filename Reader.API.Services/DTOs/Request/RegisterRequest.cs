using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Request
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
