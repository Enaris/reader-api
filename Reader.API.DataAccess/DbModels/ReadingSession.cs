using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.DataAccess.DbModels
{
    public class ReadingSession
    {
        public Guid Id { get; set; }

        public int StartLocation { get; set; }
        public int EndLocation { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Guid ReadingId { get; set; }
        public Reading Reading { get; set; }
        public Guid OptionsId { get; set; }
        public Options Options { get; set; }
    }
}
