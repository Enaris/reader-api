using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reader.API.DataAccess.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.DataAccess.Context
{
    public class ReaderContext : IdentityDbContext<AspUser>
    {
        public ReaderContext(DbContextOptions<ReaderContext> options) : base(options)
        {
        }

        public DbSet<ReaderUser> ReaderUsers { get; set; }
        public DbSet<Reading> Readings { get; set; }
        public DbSet<Options> Options { get; set; }
        public DbSet<OptionsLog> OptionsLogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ReadingTag> ReadingTags { get; set; }
        public DbSet<ReadingSession> ReadingSessions { get; set; }
    }
}
