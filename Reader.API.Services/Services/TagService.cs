using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reader.API.DataAccess.DbModels;
using Reader.API.DataAccess.Repositories;
using Reader.API.Services.DTOs;
using Reader.API.Services.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository tagRepo;
        private readonly IMapper mapper;

        public TagService(ITagRepository tagRepo, IMapper mapper)
        {
            this.tagRepo = tagRepo;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> CreateTags(IEnumerable<string> tagsNames, Guid readerUserId)
        {
            var tagsToAdd = tagsNames.Select(t => new Tag { Name = t, ReaderUserId = readerUserId }).ToList();

            tagRepo.AddRange(tagsToAdd);
            await tagRepo.SaveChangesAsync();

            return mapper.Map<IEnumerable<TagDto>>(tagsToAdd);
        }

        public async Task<IEnumerable<TagDto>> Get(Guid readerUserId)
        {
            var tagsDb = await tagRepo
                .GetAll(t => t.ReaderUserId == readerUserId)
                .ToListAsync();

            return mapper.Map<IEnumerable<TagDto>>(tagsDb);
        }
    }
}
