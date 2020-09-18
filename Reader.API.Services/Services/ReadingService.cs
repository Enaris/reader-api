using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reader.API.DataAccess.DbModels;
using Reader.API.DataAccess.Repositories;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public class ReadingService : IReadingService
    {
        private readonly IReadingRepository readingRepo;
        private readonly IMapper mapper;

        public ReadingService(IReadingRepository readingRepo, IMapper mapper)
        {
            this.readingRepo = readingRepo;
            this.mapper = mapper;
        }

        public async Task<Reading> CreateReading(ReadingAddRequest request, Guid readerUserId, string coverUrl = null)
        {
            var readingToAdd = mapper.Map<Reading>(request);

            readingToAdd.ReaderUserId = readerUserId;
            readingToAdd.CoverUrl = coverUrl;

            await readingRepo.CreateAsync(readingToAdd);
            await readingRepo.SaveChangesAsync();
            return readingToAdd;
        }

        public async Task<IEnumerable<ReadingListItem>> GetUserReadings(Guid readerUserId)
        {
            var userReadingsDb = await readingRepo
                .GetWithTags()
                .Where(r => r.ReaderUserId == readerUserId)
                .ToListAsync();

            return mapper.Map<IEnumerable<ReadingListItem>>(userReadingsDb);
        }

        public async Task<IEnumerable<ReadingListItem>> GetReadings(ReadingsRequest request, Guid readerUserId)
        {
            var userReadings = readingRepo
                .GetWithTags()
                .Where(r => r.ReaderUserId == readerUserId);

            if (!string.IsNullOrWhiteSpace(request.Title))
                userReadings = userReadings
                    .Where(r => r.Title.Contains(request.Title));

            if (request.Tags != null && request.Tags.Count() > 0)
            {
                foreach (var searchT in request.Tags)
                    userReadings = userReadings.Where(r => r.ReadingTags.Any(dbT => dbT.TagId == searchT));
            }

            var result = await userReadings.ToListAsync();

            return mapper.Map<IEnumerable<ReadingListItem>>(result);
        }

        public async Task<ReadingDetails> GetReadingDetails(Guid readerUserId, Guid readingId)
        {
            var readingDb = await readingRepo
                .GetWithTags()
                .FirstOrDefaultAsync(r => r.Id == readingId && r.ReaderUserId == readerUserId);

            return mapper.Map<ReadingDetails>(readingDb);
        }
        
        public async Task<Reading> GetReading(Guid readerUserId, Guid readingId)
        {
            return await readingRepo
                .GetAll()
                .FirstOrDefaultAsync(r => r.Id == readingId && r.ReaderUserId == readerUserId);
        }
    
        public async Task<bool> UpdateSavedLocation(Guid readerUserId, Guid readingId, int newLocation)
        {
            var readingDb = await readingRepo
                .GetWithTags()
                .FirstOrDefaultAsync(r => r.Id == readingId && r.ReaderUserId == readerUserId);

            if (readingDb == null)
                return false;

            readingDb.SavedLocation = newLocation;
            await readingRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateReading(string newCover, Reading readingDb, ReadingUpdateRequest request)
        {
            mapper.Map(request, readingDb);
            if (request.RemoveCover)
                readingDb.CoverUrl = null;
            if (newCover != null)
                readingDb.CoverUrl = newCover;
            if (request.ChangeText)
                readingDb.SavedLocation = 0;

            readingRepo.Update(readingDb);
            await readingRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReadingExist(Guid readerUserId, Guid readingId)
        {
            return await readingRepo
                .GetAll()
                .AnyAsync(r => r.Id == readingId && r.ReaderUserId == readerUserId);
        }
    }
}
