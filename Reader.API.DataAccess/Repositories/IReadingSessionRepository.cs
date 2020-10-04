using Reader.API.DataAccess.DbModels;
using System.Linq;

namespace Reader.API.DataAccess.Repositories
{
    public interface IReadingSessionRepository : IBaseRepository<ReadingSession>
    {
        IQueryable<ReadingSession> GetAllWithOptionsLog();
    }
}