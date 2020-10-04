using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response.ReadingSession;
using Reader.API.Services.DTOs.Response.ReadingSpeedGraph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public interface IReadingSessionService
    {
        Task CreateReadingSession(ReadingSessionAddRequest request);
        Task CreateReadingSession(SaveSessionRequest request, Guid optionsLogId);
        Task DeleteAllSessionsForReading(Guid readingId);
        Task<ReadingSpeedGraphData> GetSessionGraphData(Guid sessionId, Reading reading);
        Task<ReadingSpeedGraphData> GetReadingGraphData(Guid readingId);
        Task<IEnumerable<ReadingSessionDropDownItem>> GetForDropDown(Guid readingId);
    }
}