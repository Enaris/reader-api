using Microsoft.EntityFrameworkCore;
using Reader.API.DataAccess.DbModels;
using Reader.API.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public class ReadingTagService : IReadingTagService
    {
        private readonly IReadingTagRepository readingTagRepo;

        public ReadingTagService(IReadingTagRepository readingTagRepo)
        {
            this.readingTagRepo = readingTagRepo;
        }

        public async Task CreateReadingTag(ReadingTag readingTag)
        {
            await readingTagRepo.CreateAsync(readingTag);
            await readingTagRepo.SaveChangesAsync();
        }

        public async Task CreateReadingTags(IEnumerable<ReadingTag> readingTags)
        {
            readingTagRepo.AddRange(readingTags);
            await readingTagRepo.SaveChangesAsync();
        }

        public async Task DeleteReadingTags(IEnumerable<Guid> tagsIds, Guid readingId)
        {
            var tagsToRemove = await readingTagRepo
                .GetAll(t => t.ReadingId == readingId && tagsIds.Any(tToRemove => t.TagId == tToRemove))
                .ToListAsync();

            foreach (var t in tagsToRemove)
                readingTagRepo.Delete(t);

            await readingTagRepo.SaveChangesAsync();
        }
    }
}
