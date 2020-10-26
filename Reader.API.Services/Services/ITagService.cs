using Reader.API.Services.DTOs;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response.Tag;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> CreateTags(IEnumerable<string> tagsNames, Guid readerUserId);
        Task<IEnumerable<TagDto>> Get(Guid readerUserId);
        Task<IEnumerable<TagTableItem>> GetForTable(Guid readerUserId);
        Task RemoveTag(Guid tagId);
        Task<TagDetails> GetTagDetails(Guid tagId);
    }
}