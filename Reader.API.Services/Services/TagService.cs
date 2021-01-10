using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Reader.API.DataAccess.DbModels;
using Reader.API.DataAccess.Repositories;
using Reader.API.Services.DTOs;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response.Reading;
using Reader.API.Services.DTOs.Response.Tag;
using Reader.API.Services.Helpers;
using Reader.API.Services.Helpers.ReadingText;
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
        private readonly IReadingRepository readingRepo;
        private readonly IMapper mapper;

        public TagService(ITagRepository tagRepo, IReadingRepository readingRepo, IMapper mapper)
        {
            this.tagRepo = tagRepo;
            this.readingRepo = readingRepo;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> CreateTags(IEnumerable<string> tagsNames, Guid readerUserId)
        {
            var tagsToAdd = tagsNames.Select(t => new Tag { Name = t, ReaderUserId = readerUserId }).ToList();
            var result = new List<Tag>();

            //tagRepo.AddRange(tagsToAdd);
            foreach (var tag in tagsToAdd)
            {
                var tagDb = await tagRepo
                    .GetAll()
                    .FirstOrDefaultAsync(t => t.Name == tag.Name && t.ReaderUserId == tag.ReaderUserId);

                if (tagDb != null)
                    continue;

                result.Add(tag);
                await tagRepo.CreateAsync(tag);
            }
            
            await tagRepo.SaveChangesAsync();

            return mapper.Map<IEnumerable<TagDto>>(result);
        }

        public async Task<IEnumerable<TagDto>> Get(Guid readerUserId)
        {
            var tagsDb = await tagRepo
                .GetAll(t => t.ReaderUserId == readerUserId)
                .ToListAsync();

            return mapper.Map<IEnumerable<TagDto>>(tagsDb);
        }

        public async Task<IEnumerable<TagTableItem>> GetForTable(Guid readerUserId)
        {
            var tags = await tagRepo.GetAll(true).ToListAsync();

            var neededReadingsIds = new HashSet<Guid>();
            foreach (var t in tags)
                foreach (var rt in t.ReadingTags)
                    neededReadingsIds.Add(rt.ReadingId);

            var neededReadings = new List<Tuple<Reading, IEnumerable<ReadingWord>>>(neededReadingsIds.Count);     
            foreach (var rId in neededReadingsIds)
            {
                var reading = await readingRepo.Get(false, true).FirstOrDefaultAsync(r => r.Id == rId);
                var textArray = ReadingTextHelper.TextToArray(reading.Text);
                neededReadings.Add(new Tuple<Reading, IEnumerable<ReadingWord>>(reading, textArray));
            }

            var tableTags = new List<TagTableItem>(tags.Count);

            foreach (var t in tags)
            {
                var tableTag = mapper.Map<TagTableItem>(t);
                //var tagMeanCpms = new List<double>();
                //var tagMeanWpms = new List<double>();
                var tagCpms = new List<double>();
                var tagWpms = new List<double>();
                foreach (var rt in t.ReadingTags)
                {
                    var reading = neededReadings.First(r => r.Item1.Id == rt.ReadingId);
                    var readingSpeedData = ReadingTextHelper.GetReadingSpeedGraphData(reading.Item1.ReadingSessions, reading.Item2);
                    //var readingMeanCpm = readingSpeedData.Sets
                    //    .Where(s => s.SpeedType == "CPM")
                    //    .SelectMany(s => s.Points, (set, point) => point.Cpm)
                    //    .DefaultIfEmpty(-1.0)
                    //    .Average();

                    //var readingMeanWpm = readingSpeedData.Sets
                    //    .Where(s => s.SpeedType == "WPM")
                    //    .SelectMany(s => s.Points, (set, point) => point.Wpm)
                    //    .DefaultIfEmpty(-1.0)
                    //    .Average();

                    //if (readingMeanCpm != -1)
                    //    tagMeanCpms.Add(readingMeanCpm);
                    //if (readingMeanWpm != -1)
                    //    tagMeanWpms.Add(readingMeanWpm);

                    var readingCpms = readingSpeedData.Sets
                        .Where(s => s.SpeedType == "CPM")
                        .SelectMany(s => s.Points, (set, point) => point.Cpm);

                    var readingWpms = readingSpeedData.Sets
                        .Where(s => s.SpeedType == "WPM")
                        .SelectMany(s => s.Points, (set, point) => point.Wpm);

                    if (readingCpms.Any())
                        tagCpms.AddRange(readingCpms);
                    if (readingWpms.Any())
                        tagWpms.AddRange(readingWpms);
                }
                //tableTag.MeanCpm = tagMeanCpms.DefaultIfEmpty(-1).Average();
                //tableTag.MeanWpm = tagMeanWpms.DefaultIfEmpty(-1).Average();
                tableTag.MeanCpm = tagCpms.DefaultIfEmpty(-1).Average();
                tableTag.MeanWpm = tagWpms.DefaultIfEmpty(-1).Average();

                tableTags.Add(tableTag);
            }

            return tableTags;
        }
    
        public async Task RemoveTag(Guid tagId)
        {
            var tag = await tagRepo
                .GetAll()
                .FirstOrDefaultAsync(t => t.Id == tagId);

            if (tag == null)
                return;

            tagRepo.Delete(tag);
            await tagRepo.SaveChangesAsync();
        }

        public async Task<TagDetails> GetTagDetails(Guid tagId)
        {
            var tag = await tagRepo
                .GetAll(true)
                .FirstOrDefaultAsync(t => t.Id == tagId);

            if (tag == null)
                return null;

            var result = mapper.Map<TagDetails>(tag);
            var tagReadings = await readingRepo
                .Get(true, true)
                .Where(r => r.ReadingTags.Any(rt => rt.TagId == tag.Id))
                .ToListAsync();
            result.Readings = mapper.Map<IEnumerable<ReadingNameAndId>>(tagReadings);

            var tagMeanCpms = new List<double>();
            var tagMeanWpms = new List<double>();
            foreach (var r in tagReadings)
            {
                var readingSpeedData = ReadingTextHelper.GetReadingSpeedGraphData(r.ReadingSessions, r.Text);
                var readingMeanCpm = readingSpeedData.Sets
                    .Where(s => s.SpeedType == "CPM")
                    .SelectMany(s => s.Points, (set, point) => point.Cpm)
                    .DefaultIfEmpty(-1.0)
                    .Average();

                var readingMeanWpm = readingSpeedData.Sets
                    .Where(s => s.SpeedType == "WPM")
                    .SelectMany(s => s.Points, (set, point) => point.Wpm)
                    .DefaultIfEmpty(-1.0)
                    .Average();

                if (readingMeanCpm != -1)
                    tagMeanCpms.Add(readingMeanCpm);
                if (readingMeanWpm != -1)
                    tagMeanWpms.Add(readingMeanWpm);
            }
            result.MeanCpm = tagMeanCpms.DefaultIfEmpty(-1).Average();
            result.MeanWpm = tagMeanWpms.DefaultIfEmpty(-1).Average();

            return result;
        }
    }
}
