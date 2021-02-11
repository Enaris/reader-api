using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Reader.API.AutoMapper;
using Reader.API.DataAccess.Context;
using Reader.API.DataAccess.DbModels;
using Reader.API.DataAccess.Repositories;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ReaderAPI.Tests
{
    public class ReadingServiceTests
    {
        [Fact]
        public async Task CreateReading_AddReadingRequest_CreatesReading()
        {
            var options = new DbContextOptionsBuilder<ReaderContext>()
                .UseInMemoryDatabase(databaseName: "Products Test")
                .Options;

            var readingProfle = new ReadingProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(readingProfle));
            IMapper mapper = new Mapper(configuration);

            using (var context = new ReaderContext(options))
            {
                var mockReadingRepo = new Mock<ReadingRepository>(context);

                var service = new ReadingService(mockReadingRepo.Object, mapper);

                await service.CreateReading(new ReadingAddRequest
                {
                    AspUserId = new Guid(),
                    CoverImage = null,
                    Description = "Description",
                    Links = "Links",
                    Text = "Text",
                    Title = "Title"
                },
                new Guid()
                );

                mockReadingRepo.Verify(m => m.CreateAsync(It.IsAny<Reading>()), Times.Once());
                mockReadingRepo.Verify(m => m.SaveChangesAsync(), Times.Once());
            }

        }
    }
}
