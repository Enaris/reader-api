using Reader.API.DataAccess.Context;
using Reader.API.DataAccess.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.DataAccess.Repositories
{
    public class ReadingTagRepository : BaseRepository<ReadingTag>, IReadingTagRepository
    {
        public ReadingTagRepository(ReaderContext context) : base(context)
        {
        }
    }
}
