using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Response.Reading
{
    public class ReadingNameAndId
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
