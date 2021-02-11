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
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(ReaderContext context) : base(context)
        {
        }

        public IQueryable<Tag> GetAll(bool inclReadingTag = false, Guid? userId = null)
        {
            return _context.Set<Tag>()
                .IfAction(inclReadingTag, q => q.Include(t => t.ReadingTags))
                .IfAction(userId != null, q => q.Where(t => t.ReaderUserId == userId));
        }
    }
}
