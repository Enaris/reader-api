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
using Reader.API.Services.Helpers;
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
        private readonly IFileDeleteService fileDeleteService;

        public ReadingController(IUploadService uploadService, 
            ITagService tagService, 
            IReadingTagService readingTagService, 
            IReaderUserService readerUserService, 
            IReadingService readingService, 
            IFileDeleteService fileDeleteService
            )
        {
            this.uploadService = uploadService;
            this.tagService = tagService;
            this.readingTagService = readingTagService;
            this.readerUserService = readerUserService;
            this.readingService = readingService;
            this.fileDeleteService = fileDeleteService;
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

            var reading = await readingService.GetReadingDetails(userDb.Id, readingId);

            if (reading == null)
                return NotFound();

            return Ok(reading);
        }

        [HttpPost("updateReading")]
        public async Task<IActionResult> UpdateReading([FromForm] ReadingUpdateRequest request)
        {
            var userDb = await readerUserService.GetByAspId(request.AspUserId);
            if (userDb == null)
                return BadRequest(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("User", "Requesting user does not exist") });

            var readingDb = await readingService.GetReading(userDb.Id, request.ReadingId);

            if (request.RemoveCover)
                fileDeleteService.DeleteFile(readingDb.CoverUrl);

            if (request.TagsToRemove != null && request.TagsToRemove.Any())
                await readingTagService.DeleteReadingTags(request.TagsToRemove, request.ReadingId);

            var newReadingTags = new List<ReadingTag>();
            if (request.TagsToAdd != null && request.TagsToAdd.Any())
                newReadingTags = (await tagService.CreateTags(request.TagsToAdd, userDb.Id))
                    .Select(tDto => new ReadingTag { ReadingId = request.ReadingId, TagId = tDto.Id })
                    .ToList();

            if (request.TagsToAssign != null && request.TagsToAssign.Any())
                newReadingTags
                    .AddRange(request.TagsToAssign.Select(t => new ReadingTag { ReadingId = request.ReadingId, TagId = t }));

            if (newReadingTags.Any())
                await readingTagService.CreateReadingTags(newReadingTags);

            string newCover = null;
            if (request.NewCoverImage != null)
                newCover = await uploadService.UploadImage(request.NewCoverImage, userDb.UserAspId);
            await readingService.UpdateReading(newCover, readingDb, request);

            return Ok();
        }
    
        [HttpPost("user/{aspUserId}/delete/{readingId}")]
        public async Task<IActionResult> DeleteReading([FromRoute] Guid aspUserId, [FromRoute] Guid readingId)
        {
            var userDb = await readerUserService.GetByAspId(aspUserId);

            if (userDb == null)
                return BadRequest(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("User", "Requesting user does not exist") });

            var readingDb = await readingService.GetReading(userDb.Id, readingId);
            if (readingDb == null)
                return Ok();

            if (readingDb.CoverUrl != null)
                fileDeleteService.DeleteFile(readingDb.CoverUrl);

            await readingService.RemoveReading(readingDb);
            return Ok();
        }
    }
}