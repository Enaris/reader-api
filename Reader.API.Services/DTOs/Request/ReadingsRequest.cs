using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Request
{
    public class ReadingsRequest
    {
        public string Title { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
    }
}
