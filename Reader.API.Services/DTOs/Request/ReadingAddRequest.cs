using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Request
{
    public class ReadingAddRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public IFormFile CoverImage { get; set; }
        public string Description { get; set; }
        public string Links { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public IEnumerable<string> NewTagsNames { get; set; }

        public Guid AspUserId { get; set; }
    }

    public class ReadingAddRequestValidator : AbstractValidator<ReadingAddRequest>
    {
        public ReadingAddRequestValidator()
        {
            RuleFor(r => r.Title)
                .NotEmpty().WithMessage("Title is required");
            RuleFor(r => r.Text)
                .NotEmpty().WithMessage("Text is required");
            RuleFor(r => r.Description)
                .MaximumLength(254).WithMessage("Description is too long");
            RuleFor(r => r.Links)
                .MaximumLength(254).WithMessage("Links are too long");
        }
    }
}
