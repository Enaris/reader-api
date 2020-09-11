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
}
