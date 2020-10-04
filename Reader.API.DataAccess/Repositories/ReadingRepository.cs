using Microsoft.EntityFrameworkCore;
using Reader.API.DataAccess.Context;
using Reader.API.DataAccess.DbModels;
using Reader.API.DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reader.API.DataAccess.Repositories
{
    public class ReadingRepository : BaseRepository<Reading>, IReadingRepository
    {
        public ReadingRepository(ReaderContext context) : base(context)
        {
        }

        public IQueryable<Reading> GetWithTags()
        {
            return _context.Set<Reading>()
                .Include(r => r.ReadingTags)
                    .ThenInclude(rt => rt.Tag);
        }

        public IQueryable<Reading> Get(bool inclTags = false, bool inclSessions = false)
        {
            var readings = _context.Set<Reading>()
                .IfAction(inclTags, q => q.Include(r => r.ReadingTags).ThenInclude(qq => qq.Tag))
                .IfAction(inclSessions, q => q.Include(r => r.ReadingSessions).ThenInclude(qq => qq.OptionsLog));



            //if (inclTags)
            //    readings.Include(r => r.ReadingTags)
            //        .ThenInclude(rt => rt.Tag);

            //if (inclSessions)
            //    readings.Include(r => r.ReadingSessions)
            //        .ThenInclude(rs => rs.OptionsLog);

            return readings;
        }

    }
}
