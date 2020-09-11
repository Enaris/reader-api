using Reader.API.DataAccess.Context;
using Reader.API.DataAccess.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.DataAccess.Repositories
{
    public class ReaderUserRepository : BaseRepository<ReaderUser>, IReaderUserRepository
    {
        public ReaderUserRepository(ReaderContext context) : base(context)
        {
        }

    }
}
