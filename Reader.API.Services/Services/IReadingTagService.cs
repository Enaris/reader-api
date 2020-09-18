using Reader.API.DataAccess.DbModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public interface IReadingTagService
    {
        Task CreateReadingTag(ReadingTag readingTag);
        Task CreateReadingTags(IEnumerable<ReadingTag> readingTags);
        Task DeleteReadingTags(IEnumerable<Guid> tagsIds, Guid readingId);
    }
}