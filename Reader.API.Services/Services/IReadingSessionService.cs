using Reader.API.Services.DTOs.Request;
using System;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public interface IReadingSessionService
    {
        Task CreateReadingSession(ReadingSessionAddRequest request);
        Task CreateReadingSession(SaveSessionRequest request, Guid optionsLogId);
    }
}