using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Response.ReadingSpeedGraph
{
    public class ReadingSpeedGraphSet
    {
        public string SpeedType { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public IEnumerable<ReadingSpeedGraphPoint> Points { get; set; }
    }
}
