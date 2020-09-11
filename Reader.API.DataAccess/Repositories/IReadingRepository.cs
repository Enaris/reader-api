using Reader.API.DataAccess.DbModels;
using System.Linq;

namespace Reader.API.DataAccess.Repositories
{
    public interface IReadingRepository : IBaseRepository<Reading>
    {
        IQueryable<Reading> GetWithTags();
    }
}