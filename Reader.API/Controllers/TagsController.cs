using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reader.API.Services.Services;

namespace Reader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService tagService;
        private readonly IReaderUserService readerUserService;

        public TagsController(ITagService tagService, IReaderUserService readerUserService)
        {
            this.tagService = tagService;
            this.readerUserService = readerUserService;
        }

        [HttpGet("{aspUserId}")]
        public async Task<IActionResult> GetUserTags(Guid aspUserId)
        {
            var userDb = await readerUserService.GetByAspId(aspUserId);

            if (userDb == null)
                return BadRequest(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("User", "User does not seem to exist") });

            var tags = await tagService.Get(userDb.Id);

            return Ok(tags);
        }
    }
}