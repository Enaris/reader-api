using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Request
{
    public class ReadingUpdateRequest
    {
        public Guid AspUserId { get; set; }
        public Guid ReadingId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public IFormFile NewCoverImage { get; set; }
        public string Description { get; set; }
        public string Links { get; set; }

        public bool ChangeText { get; set; }
        public bool RemoveCover { get; set; }

        public IEnumerable<string> TagsToAdd { get; set; }
        public IEnumerable<Guid> TagsToAssign { get; set; }
        public IEnumerable<Guid> TagsToRemove { get; set; }
    }

    public class ReadingUpdateRequestValidator : AbstractValidator<ReadingUpdateRequest>
    {
        public ReadingUpdateRequestValidator()
        {
            RuleFor(r => r.Title)
                .NotEmpty().WithMessage("Title is required");
            When(r => r.ChangeText, () =>
            {
                RuleFor(r => r.Text)
                    .NotEmpty().WithMessage("Text is required");
            });
            RuleFor(r => r.Description)
                .MaximumLength(254).WithMessage("Description is too long");
            RuleFor(r => r.Links)
                .MaximumLength(254).WithMessage("Links are too long");
        }
    }
}
