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
    public class ReadingSessionServiceTests
    {
        [Fact]
        public async Task CreateReadingSession_SessionSaveRequest_CreatesSession()
        {
            var options = new DbContextOptionsBuilder<ReaderContext>()
                .UseInMemoryDatabase(databaseName: "Products Test")
                .Options;

            var readingProfle = new ReadingSessionProfiles();
            var optionsProfile = new OptionsLogProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile> { readingProfle, optionsProfile }));
            IMapper mapper = new Mapper(configuration);

            using (var context = new ReaderContext(options))
            {
                var mockReadingSessionRepo = new Mock<ReadingSessionRepository>(context);
                var mockReadingRepo = new Mock<ReadingRepository>(context);

                var service = new ReadingSessionService(mockReadingSessionRepo.Object, mockReadingRepo.Object, mapper);

                var saveSessionRequest = new SaveSessionRequest
                {
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddSeconds(3),
                    ReadingId = new Guid(),
                    StartLocation = 0,
                    EndLocation = 10,
                    OptionsLog = new OptionsLogAddRequest
                    {
                        AddPerMin = -1,
                        AppendIfShorter = -1,
                        BreakIfLonger = -1,
                        InitialAccelerationTimeSecs = 60,
                        InitialCPM = -1,
                        InitialWPM = 300,
                        MaxAppend = -1,
                        SlowIfLonger = -1,
                        SlowTo = -1,
                        TargetCPM = -1,
                        TargetWPM = 400
                    }
                };

                await service.CreateReadingSession(saveSessionRequest, new Guid());

                mockReadingSessionRepo.Verify(m => m.CreateAsync(It.IsAny<ReadingSession>()), Times.Once());
                mockReadingSessionRepo.Verify(m => m.SaveChangesAsync(), Times.Once());
            }

        }
    }
}
