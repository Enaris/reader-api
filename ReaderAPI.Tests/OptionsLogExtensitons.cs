using Reader.API.DataAccess.DbModels;
using Reader.API.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ReaderAPI.Tests
{
    public class OptionsLogExtensitons
    {
        [Theory]
        [InlineData(-1, 1, "CPM")]
        [InlineData(1, -1, "WPM")]
        [InlineData(int.MaxValue, -1, "WPM")]
        [InlineData(-1, int.MaxValue, "CPM")]
        public void GetSpeedType_OptionsLog_CorrectSpeedType(int wpm, int cpm, string expected)
        {
            var options = new OptionsLog
            {
                InitialCPM = cpm,
                InitialWPM = wpm
            };

            var result = options.GetSpeedType();
            
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1, 100, 100)]
        [InlineData(100, -1, 100)]
        public void GetInitialSpeed_OptionsLog_CorrectSpeedValue(int wpm, int cpm, int expected)
        {
            var options = new OptionsLog
            {
                InitialCPM = cpm,
                InitialWPM = wpm
            };

            var result = options.GetInitialSpeed();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1, 100, 100)]
        [InlineData(100, -1, 100)]
        public void GetTargetSpeed_OptionsLog_CorrectSpeedValue(int wpm, int cpm, int expected)
        {
            var options = new OptionsLog
            {
                InitialCPM = cpm,
                InitialWPM = wpm, 
                TargetCPM = cpm, 
                TargetWPM = wpm
            };

            var result = options.GetTargetSpeed();

            Assert.Equal(expected, result);
        }

    }
}
