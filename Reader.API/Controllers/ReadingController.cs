using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.Services;

namespace Reader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadingController : ControllerBase
    {
        private readonly IUploadService uploadService;
        private readonly ITagService tagService;
        private readonly IReadingTagService readingTagService;
        private readonly IReaderUserService readerUserService;
        private readonly IReadingService readingService;

        public ReadingController(IUploadService uploadService, 
            ITagService tagService, 
            IReadingTagService readingTagService, 
            IReaderUserService readerUserService, 
            IReadingService readingService
            )
        {
            this.uploadService = uploadService;
            this.tagService = tagService;
            this.readingTagService = readingTagService;
            this.readerUserService = readerUserService;
            this.readingService = readingService;
        }

        [HttpPost("addReading")]
        public async Task<IActionResult> Create([FromForm] ReadingAddRequest request)
        {
            var userDb = await readerUserService.GetByAspId(request.AspUserId);

            if (userDb == null)
                return BadRequest(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("User", "User does not exist") });

            string coverUrl = null;

            if (request.CoverImage != null)
                coverUrl = await uploadService.UploadImage(request.CoverImage, request.AspUserId);

            var readingTagsToAdd = new List<TagDto>();
            if (request.NewTagsNames != null && request.NewTagsNames.Any())
                readingTagsToAdd = (await tagService.CreateTags(request.NewTagsNames, userDb.Id)).ToList();

            if (request.Tags != null && request.Tags.Any())
                readingTagsToAdd.AddRange(request.Tags.Select(t => new TagDto { Id = t }));

            var readingDb = await readingService.CreateReading(request, userDb.Id, coverUrl);

            var readingTags = readingTagsToAdd.Select(t => new ReadingTag { ReadingId = readingDb.Id, TagId = t.Id });
            if (readingTags.Any())
                await readingTagService.CreateReadingTags(readingTags);

            return Ok();
        }

        //[HttpGet("user/{aspUserId}")]
        //public async Task<IActionResult> GetUserReadings([FromRoute] Guid aspUserId)
        //{
        //    var userDb = await readerUserService.GetByAspId(aspUserId);

        //    if (userDb == null)
        //        return BadRequest(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("User", "Requesting user does not exist") });

        //    var readings = await readingService.GetUserReadings(userDb.Id);

        //    return Ok(readings);
        //}
        
        [HttpGet("user/{aspUserId}")]
        public async Task<IActionResult> GetReadings([FromRoute] Guid aspUserId, 
            [FromQuery(Name = "tags[]")] IEnumerable<Guid> tags, 
            [FromQuery(Name = "title")] string title  
            )
        {
            var userDb = await readerUserService.GetByAspId(aspUserId);

            if (userDb == null)
                return BadRequest(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("User", "Requesting user does not exist") });

            var readings = await readingService.GetReadings(new ReadingsRequest { Tags = tags, Title = title }, userDb.Id);

            return Ok(readings);
        }

        [HttpGet("user/{aspUserId}/reading/{readingId}")]
        public async Task<IActionResult> GetReading([FromRoute] Guid aspUserId, [FromRoute] Guid readingId)
        {
            var userDb = await readerUserService.GetByAspId(aspUserId);

            if (userDb == null)
                return BadRequest(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("User", "Requesting user does not exist") });

            var reading = await readingService.GetReading(userDb.Id, readingId);

            if (reading == null)
                return NotFound();

            return Ok(reading);
        }
    }
}