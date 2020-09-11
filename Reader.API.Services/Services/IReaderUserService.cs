using Reader.API.DataAccess.DbModels;
using System;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public interface IReaderUserService
    {
        Task<ReaderUser> GetByAspId(Guid aspId, bool withAspUser = false);
        Task<ReaderUser> Create(Guid aspUserId);
    }
}