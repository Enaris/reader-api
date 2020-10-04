using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Response.ReadingSession
{
    public class ReadingSessionDropDownItem
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int WordsRead { get; set; }
        public string SpeedType { get; set; }
    }
}
