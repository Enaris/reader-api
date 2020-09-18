using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public interface IReadingService
    {
        Task<IEnumerable<ReadingListItem>> GetReadings(ReadingsRequest request, Guid readerUserId);
        Task<Reading> CreateReading(ReadingAddRequest request, Guid readerUserId, string coverUrl = null);
        Task<IEnumerable<ReadingListItem>> GetUserReadings(Guid readerUserId);
        Task<ReadingDetails> GetReadingDetails(Guid readerUserId, Guid readingId);
        Task<bool> UpdateSavedLocation(Guid readerUserId, Guid readingId, int newLocation);
        Task<bool> UpdateReading(string newCover, Reading readingDb, ReadingUpdateRequest request);
        Task<bool> ReadingExist(Guid readerUserId, Guid readingId);
        Task<Reading> GetReading(Guid readerUserId, Guid readingId);
    }
}