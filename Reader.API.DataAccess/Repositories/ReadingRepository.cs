using Microsoft.EntityFrameworkCore;
using Reader.API.DataAccess.Context;
using Reader.API.DataAccess.DbModels;
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

    }
}
