using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Response
{
    public class ReadingCreated
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string CoverUrl { get; set; }
        public string Description { get; set; }
        public string Links { get; set; }
        public int SavedLocation { get; set; }

        public Guid AspUserId { get; set; }
    }
}
