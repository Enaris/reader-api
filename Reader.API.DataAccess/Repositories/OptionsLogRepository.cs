using Reader.API.DataAccess.Context;
using Reader.API.DataAccess.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.DataAccess.Repositories
{
    public class OptionsLogRepository : BaseRepository<OptionsLog>, IOptionsLogRepository
    {
        public OptionsLogRepository(ReaderContext context) : base(context)
        {
        }

    }
}
