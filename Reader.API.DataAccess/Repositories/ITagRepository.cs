using Reader.API.DataAccess.DbModels;
using System;
using System.Linq;

namespace Reader.API.DataAccess.Repositories
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        IQueryable<Tag> GetAll(bool inclReadingTag = false, Guid? userId = null);
    }
}