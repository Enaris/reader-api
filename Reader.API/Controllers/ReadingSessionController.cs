using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.Services;

namespace Reader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadingSessionController : ControllerBase
    {
        private readonly IReadingSessionService readingSessionService;
        private readonly IOptionsLogService optionsLogService;
        private readonly IReadingService readingService;
        private readonly IReaderUserService readerUserService;

        public ReadingSessionController(IReadingSessionService readingSessionService, 
            IOptionsLogService optionsLogService, 
            IReadingService readingService, 
            IReaderUserService readerUserService)
        {
            this.readingSessionService = readingSessionService;
            this.optionsLogService = optionsLogService;
            this.readingService = readingService;
            this.readerUserService = readerUserService;
        }

        [HttpPost("{aspUserId}/saveSession")]
        public async Task<IActionResult> SaveSession([FromRoute] Guid aspUserId, [FromBody] SaveSessionRequest request)
        {
            var userDb = await readerUserService.GetByAspId(aspUserId);
            if (userDb == null)
                return BadRequest(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("User", "Requesting user does not exist") });

            var updatedReadingLocation = await readingService.UpdateSavedLocation(userDb.Id, request.ReadingId, request.EndLocation);
            if (!updatedReadingLocation)
                return BadRequest(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("Reading", "Reading to update does not exist") });

            var addedOptionsLog = await optionsLogService.CreateOptionsLog(request.OptionsLog);
            await readingSessionService.CreateReadingSession(request, addedOptionsLog.Id);

            return Ok();
        }
    }
}