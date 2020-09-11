using Microsoft.EntityFrameworkCore;
using Reader.API.DataAccess.DbModels;
using Reader.API.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public class ReaderUserService : IReaderUserService
    {
        private readonly IReaderUserRepository readerUserRepo;

        public ReaderUserService(IReaderUserRepository readerUserRepo)
        {
            this.readerUserRepo = readerUserRepo;
        }

        public async Task<ReaderUser> GetByAspId(Guid aspId, bool withAspUser = false)
        {
            return
                withAspUser ?
                await readerUserRepo
                    .GetAll()
                    .Include(u => u.AspUser)
                    .FirstOrDefaultAsync(u => u.UserAspId == aspId)
                :
                await readerUserRepo
                    .GetAll()
                    .FirstOrDefaultAsync(u => u.UserAspId == aspId);
        }

        public async Task<ReaderUser> Create(Guid aspUserId)
        {
            var userToAdd = new ReaderUser { UserAspId = aspUserId };
            await readerUserRepo.CreateAsync(userToAdd);
            await readerUserRepo.SaveChangesAsync();
            return userToAdd;
        }
    }
}
