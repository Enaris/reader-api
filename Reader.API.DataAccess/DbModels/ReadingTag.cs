using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.DataAccess.DbModels
{
    public class ReadingTag
    {
        public Guid Id { get; set; }

        public Guid ReadingId { get; set; }
        public Reading Reading { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
