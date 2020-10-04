using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Response.ReadingSpeedGraph
{
    public class ReadingSpeedGraphData
    {
        public bool Empty { get; set; } = false;
        public long AllCharactersCount { get; set; }
        public IEnumerable<ReadingSpeedGraphSet> Sets { get; set; }
    }
}
