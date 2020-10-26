using Reader.API.Services.DTOs.Response.Reading;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Response.Tag
{
    public class TagDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public int TagedCount { get; set; }
        public double MeanWpm { get; set; }
        public double MeanCpm { get; set; }

        public IEnumerable<ReadingNameAndId> Readings { get; set; }
    }
}
