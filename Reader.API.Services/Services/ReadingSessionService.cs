using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reader.API.DataAccess.DbModels;
using Reader.API.DataAccess.Repositories;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response.ReadingSession;
using Reader.API.Services.DTOs.Response.ReadingSpeedGraph;
using Reader.API.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public class ReadingSessionService : IReadingSessionService
    {
        private readonly IReadingSessionRepository readingSessionRepo;
        private readonly IReadingRepository readingRepo;
        private readonly IMapper mapper;

        public ReadingSessionService(IReadingSessionRepository readingSessionRepo, IReadingRepository readingRepo, IMapper mapper)
        {
            this.readingSessionRepo = readingSessionRepo;
            this.readingRepo = readingRepo;
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

        public async Task DeleteAllSessionsForReading(Guid readingId)
        {
            var sessionsToRemove = await readingSessionRepo
                .GetAll(rs => rs.ReadingId == readingId)
                .ToListAsync();

            foreach (var rs in sessionsToRemove)
                readingSessionRepo.Delete(rs);

            await readingSessionRepo.SaveChangesAsync();
        }

        public async Task<ReadingSpeedGraphData> GetSessionGraphData(Guid sessionId, Reading reading)
        {
            //var reading = await readingRepo
            //    .Get()
            //    .FirstOrDefaultAsync(r => r.Id == readingId);

            //if (reading == null)
            //    return null;

            var session = await readingSessionRepo
                .GetAllWithOptionsLog()
                .FirstOrDefaultAsync(s => s.Id == sessionId && s.ReadingId == reading.Id);

            if (session == null)
                return null;

            return ReadingTextHelper.GetReadingSpeedGraphData(new List<ReadingSession> { session }, reading.Text);
        }

        public async Task<ReadingSpeedGraphData> GetReadingGraphData(Guid readingId)
        {
            var reading = await readingRepo
                .Get(false, true)
                .FirstOrDefaultAsync(r => r.Id == readingId);

            if (reading == null)
                return null;

            if (reading.ReadingSessions.Count == 0)
                return new ReadingSpeedGraphData { Empty = true };

            return ReadingTextHelper.GetReadingSpeedGraphData(reading.ReadingSessions, reading.Text);
        }

        public async Task<IEnumerable<ReadingSessionDropDownItem>> GetForDropDown(Guid readingId)
        {
            var sessionsDb = await readingSessionRepo
                .GetAllWithOptionsLog()
                .Where(rs => rs.ReadingId == readingId)
                .ToListAsync();

            return mapper.Map<IEnumerable<ReadingSessionDropDownItem>>(sessionsDb);
        }

    }
}
