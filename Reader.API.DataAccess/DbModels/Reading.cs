using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Reader.API.DataAccess.DbModels
{
    public class Reading
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        public string CoverUrl { get; set; }
        public string Description { get; set; }
        public string Links { get; set; }
        public int SavedLocation { get; set; }

        public Guid ReaderUserId { get; set; }
        public ReaderUser ReaderUser { get; set; }
        public virtual ICollection<ReadingSession> ReadingSessions { get; set; }
        public virtual ICollection<ReadingTag> ReadingTags { get; set; }
    }
}
