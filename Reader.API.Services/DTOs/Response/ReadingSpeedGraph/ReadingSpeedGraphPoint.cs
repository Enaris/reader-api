using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Response.ReadingSpeedGraph
{
    public class ReadingSpeedGraphPoint
    {
        public double Wpm { get; set; }
        public double Cpm { get; set; }
        public int WordStart { get; set; }
        public long MsFromStart { get; set; }
    }
}
