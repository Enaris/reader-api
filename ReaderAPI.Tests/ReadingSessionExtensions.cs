using Reader.API.DataAccess.DbModels;
using Reader.API.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ReaderAPI.Tests
{
    public class ReadingSessionExtensions
    {
        [Theory]
        [InlineData(0, 30, 30)]
        [InlineData(int.MaxValue - 100, int.MaxValue, 100)]
        public void GetWordsRead_OptionsLog_CorrectSpeedType(int start, int end, int expected)
        {
            var session = new ReadingSession
            {
                StartLocation = start, 
                EndLocation = end
            };

            var result = session.GetWordsRead();

            Assert.Equal(expected, result);
        }

    }
}
