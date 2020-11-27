using FluentValidation;
using Reader.API.Services.DTOs.Response;
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

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(r => r.Email)
                .EmailAddress().WithMessage("Enter valid email address")
                .NotEmpty().WithMessage("Email is required");
            RuleFor(r => r.Password)
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .MaximumLength(12).WithMessage("Passowrd cannot be longer than 12 characters");
        }
    }

}
