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
        Task<ReadingDetails> GetReading(Guid readerUserId, Guid readingId);
    }
}