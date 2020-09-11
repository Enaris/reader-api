using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Request
{
    public class ReadingSessionAddRequest
    {
        public int StartLocation { get; set; }
        public int EndLocation { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Guid ReadingId { get; set; }
        public Guid OptionsLogId { get; set; }
    }
}
