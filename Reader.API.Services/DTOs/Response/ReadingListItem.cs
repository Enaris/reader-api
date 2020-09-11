using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Response
{
    public class ReadingListItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CoverUrl { get; set; }
        public int SavedLocations { get; set; }

        public IEnumerable<TagDto> Tags { get; set; }
    }
}
