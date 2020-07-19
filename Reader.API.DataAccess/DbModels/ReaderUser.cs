using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.DataAccess.DbModels
{
    public class ReaderUser
    {
        public Guid Id { get; set; }

        public Guid UserAspId { get; set; }
        public AspUser AspUser { get; set; }

        public virtual ICollection<Reading> Readings { get; set; }
        public virtual ICollection<Options> Options { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
