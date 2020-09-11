using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Request
{
    public class SaveSessionRequest
    {
        public int StartLocation { get; set; }
        public int EndLocation { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public OptionsLogAddRequest OptionsLog { get; set; }
        public Guid ReadingId { get; set; }
    }
}
