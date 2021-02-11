using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;
using Reader.API.DataAccess.Context;
using Reader.API.DataAccess.DbModels;
using Reader.API.DataAccess.Repositories;
using Reader.API.Services.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ReaderAPI.Tests
{
    public class TagServiceTests
    {
        
        [Fact]
        public async Task CreateTags_Tag_CreatesTags()
        {
            var options = new DbContextOptionsBuilder<ReaderContext>()
                .UseInMemoryDatabase(databaseName: "Products Test")
                .Options;

            using (var context = new ReaderContext(options))
            {
                var mockTagRepo = new Mock<TagRepository>(context);
                var mockReadingRepo = new Mock<ReadingRepository>(context);
                var mapper = new Mock<IMapper>();

                var service = new TagService(mockTagRepo.Object, mockReadingRepo.Object, mapper.Object);

                await service.CreateTags(new List<string> { "a" }, new Guid());

                mockTagRepo.Verify(m => m.CreateAsync(It.IsAny<Tag>()), Times.Once());
                mockTagRepo.Verify(m => m.SaveChangesAsync(), Times.Once());
            }

        }
    }
}
