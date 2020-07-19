using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.DataAccess.DbModels
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid? ReaderUserId { get; set; }
        public ReaderUser ReaderUser { get; set; }

        public virtual ICollection<ReadingTag> ReadingTags { get; set; }
    }
}
