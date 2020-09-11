using AutoMapper;
using Reader.API.DataAccess.DbModels;
using Reader.API.DataAccess.Repositories;
using Reader.API.Services.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public class ReadingSessionService : IReadingSessionService
    {
        private readonly IReadingSessionRepository readingSessionRepo;
        private readonly IMapper mapper;

        public ReadingSessionService(IReadingSessionRepository readingSessionRepo, IMapper mapper)
        {
            this.readingSessionRepo = readingSessionRepo;
            this.mapper = mapper;
        }

        public async Task CreateReadingSession(ReadingSessionAddRequest request)
        {
            var readingSessionToAdd = mapper.Map<ReadingSession>(request);
            await readingSessionRepo.CreateAsync(readingSessionToAdd);
            await readingSessionRepo.SaveChangesAsync();
        }

        public async Task CreateReadingSession(SaveSessionRequest request, Guid optionsLogId)
        {
            var readingSessionToAdd = mapper.Map<ReadingSession>(request);
            readingSessionToAdd.OptionsLogId = optionsLogId;
            await readingSessionRepo.CreateAsync(readingSessionToAdd);
            await readingSessionRepo.SaveChangesAsync();
        }
    }
}
