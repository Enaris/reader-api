using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs.Response.ReadingSpeedGraph;
using Reader.API.Services.Helpers;
using Reader.API.Services.Helpers.ReadingText;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ReaderAPI.Tests
{
    public class ReadingText
    {
        [Theory]
        [InlineData(true, 0, 0, false)]
        [InlineData(true, -1, -1, false)]
        [InlineData(true, long.MaxValue, long.MaxValue, false)]
        [InlineData(true, long.MaxValue, long.MaxValue - 1, true)]
        public void ShouldDoConstAcc_TrueConditions_True(bool doConstAcc,
            long msPassed, long iniAccTimeMs, bool doIniAcc)
        {
            var result = ReadingTextHelper
                .ShouldDoConstAcc(doConstAcc, msPassed, iniAccTimeMs, doIniAcc);
            Assert.True(result);
        }

        [Theory]
        [InlineData(true, 0, 1, true)]
        [InlineData(false, -1, -1, true)]
        [InlineData(false, -1, -1, false)]
        [InlineData(false, long.MaxValue, long.MaxValue - 1, false)]
        public void ShouldDoConstAcc_FalseConditions_False(bool doConstAcc,
            long msPassed, long iniAccTimeMs, bool doIniAcc)
        {
            var result = ReadingTextHelper
                .ShouldDoConstAcc(doConstAcc, msPassed, iniAccTimeMs, doIniAcc);
            Assert.False(result);
        }

        [Theory]
        [InlineData(true, 1, 0, 0, 1)]
        [InlineData(true, long.MaxValue, long.MaxValue - 1, double.MaxValue - 1, double.MaxValue)]
        public void ShouldDoIniAcc_TrueConditions_True(bool doIniAcc,
            long iniAccTime, long msPassed, double currentSpeed, double targetSpeed)
        {
            var result = ReadingTextHelper
                .ShouldDoIniAcc(doIniAcc, iniAccTime, msPassed, currentSpeed, targetSpeed);
            Assert.True(result);
        }

        [Theory]
        [InlineData(false, 1, 0, 0, 1)]
        [InlineData(false, -1, -1, -1, -1)]
        [InlineData(true, long.MaxValue - 1, long.MaxValue, double.MaxValue, double.MaxValue)]
        [InlineData(true, long.MaxValue, long.MaxValue, double.MaxValue, double.MaxValue - 1)]
        public void ShouldDoIniAcc_FalseConditions_False(bool doIniAcc,
            long iniAccTime, long msPassed, double currentSpeed, double targetSpeed)
        {
            var result = ReadingTextHelper
                .ShouldDoIniAcc(doIniAcc, iniAccTime, msPassed, currentSpeed, targetSpeed);
            Assert.False(result);
        }

        [Theory]
        [InlineData(true, 1, 2)]
        [InlineData(true, int.MaxValue - 1, int.MaxValue)]
        public void ShouldSlow_TrueConditions_True(bool doSlow, int slowIfLonger, int overallLength)
        {
            var result = ReadingTextHelper
                .ShouldSlow(doSlow, slowIfLonger, overallLength);
            Assert.True(result);
        }

        [Theory]
        [InlineData(false, 1, 2)]
        [InlineData(false, int.MaxValue - 1, int.MaxValue)]
        [InlineData(false, int.MinValue, int.MinValue)]
        [InlineData(true, 2, 1)]
        [InlineData(true, int.MaxValue, int.MaxValue - 1)]
        public void ShouldSlow_FalseConditions_False(bool doSlow, int slowIfLonger, int overallLength)
        {
            var result = ReadingTextHelper
                .ShouldSlow(doSlow, slowIfLonger, overallLength);
            Assert.False(result);
        }

        [Theory]
        [InlineData(true, 0, 1, 2, 1, false, 0, 1)]
        [InlineData(true, int.MaxValue - 1, int.MaxValue, int.MaxValue, int.MaxValue - 1, false, int.MinValue, int.MaxValue - 1)]
        [InlineData(true, 0, 1, 2, 1, true, 2, 1)]
        [InlineData(true, int.MaxValue - 1, int.MaxValue, int.MaxValue, int.MaxValue - 1, true, int.MaxValue, int.MaxValue - 1)]
        public void ShouldAppend_TrueConditions_True(bool doAppend,
            int appended,
            int maxAppend,
            int appendIfShorter,
            int wordLength,
            bool doBreak,
            int breakIfLonger,
            int overallLength)
        {
            var result = ReadingTextHelper
                .ShouldAppend(doAppend,
                    appended,
                    maxAppend,
                    appendIfShorter,
                    wordLength,
                    doBreak,
                    breakIfLonger,
                    overallLength);
            Assert.True(result);
        }

        [Theory]
        [InlineData(false, 0, 1, 2, 1, false, 0, 1)]
        [InlineData(false, int.MaxValue - 1, int.MaxValue, int.MaxValue, int.MaxValue - 1, false, int.MinValue, int.MaxValue - 1)]
        [InlineData(false, 0, 1, 2, 1, true, 2, 1)]
        [InlineData(false, int.MaxValue - 1, int.MaxValue, int.MaxValue, int.MaxValue - 1, true, int.MaxValue, int.MaxValue - 1)]
        [InlineData(true, 1, 0, 1, 2, false, 1, 0)]
        [InlineData(true, int.MaxValue, int.MaxValue - 1, int.MaxValue - 1, int.MaxValue, false, int.MinValue, int.MaxValue - 1)]
        [InlineData(true, 1, 0, 1, 2, true, 1, 2)]
        [InlineData(true, int.MaxValue - 1, int.MaxValue, int.MaxValue, int.MaxValue - 1, true, int.MaxValue - 1, int.MaxValue)]
        [InlineData(true, 2, 1, 2, 1, true, 1, 2)]
        public void ShouldAppend_FalseConditions_False(bool doAppend,
            int appended,
            int maxAppend,
            int appendIfShorter,
            int wordLength,
            bool doBreak,
            int breakIfLonger,
            int overallLength)
        {
            var result = ReadingTextHelper
                .ShouldAppend(doAppend,
                    appended,
                    maxAppend,
                    appendIfShorter,
                    wordLength,
                    doBreak,
                    breakIfLonger,
                    overallLength);
            Assert.False(result);
        }

        [Theory]
        [InlineData(true, 1, 2)]
        [InlineData(true, int.MaxValue - 1, int.MaxValue)]
        public void ShouldBreak_TrueConditions_True(bool doBreak, int breakIfLonger, int overallLength)
        {
            var result = ReadingTextHelper
                .ShouldBreak(doBreak, breakIfLonger, overallLength);
            Assert.True(result);
        }

        [Theory]
        [InlineData(true, 2, 1)]
        [InlineData(true, int.MaxValue, int.MaxValue - 1)]
        [InlineData(false, 1, 2)]
        [InlineData(false, int.MaxValue - 1, int.MaxValue)]
        public void ShouldBreak_FalseConditions_False(bool doBreak, int breakIfLonger, int overallLength)
        {
            var result = ReadingTextHelper
                .ShouldBreak(doBreak, breakIfLonger, overallLength);
            Assert.False(result);
        }

        [Theory]
        [InlineData(true, 1, 0)]
        [InlineData(true, 0, 1)]
        [InlineData(true, int.MaxValue - 1, int.MaxValue)]
        [InlineData(true, int.MinValue, int.MinValue + 1)]
        public void DidBreak_TrueConditions_True(bool doBreak, int wordEnd, int lastEnd)
        {
            var result = ReadingTextHelper
                .DidBreak(doBreak, wordEnd, lastEnd);
            Assert.True(result);
        }

        [Theory]
        [InlineData(true, 1, 1)]
        [InlineData(true, int.MaxValue, int.MaxValue)]
        [InlineData(false, 1, 0)]
        [InlineData(false, 0, 1)]
        [InlineData(false, int.MaxValue - 1, int.MaxValue)]
        [InlineData(false, int.MinValue, int.MinValue + 1)]
        public void DidBreak_FalseConditions_False(bool doBreak, int wordEnd, int lastEnd)
        {
            var result = ReadingTextHelper
                .DidBreak(doBreak, wordEnd, lastEnd);
            Assert.False(result);
        }

        [Theory]
        [InlineData(200, 300)]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        [InlineData(int.MinValue, 0)]
        public void WaitToWpm_WaitInMs_Wpm(int wait, int expetedWPM)
        {
            var result = ReadingTextHelper.WaitToWpm(wait);
            Assert.Equal(expetedWPM, result);
        }

        [Theory]
        [InlineData(200)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void WaitToWpm_WaitInMs_WpmAsInt(int wait)
        {
            var result = ReadingTextHelper.WaitToWpm(wait);
            Assert.IsType<int>(result);
        }

        [Theory]
        [InlineData(200, 6, 1800)]
        [InlineData(0, 6, 0)]
        [InlineData(200, 0, 0)]
        [InlineData(int.MinValue, 0, 0)]
        public void WaitToCpm_WaitInMs_Cpm(int wait, int word_length, int expetedCpm)
        {
            var result = ReadingTextHelper.WaitToCpm(wait, word_length);
            Assert.Equal(expetedCpm, result);
        }

        [Theory]
        [InlineData(200, 6)]
        [InlineData(0, 6)]
        [InlineData(200, 0)]
        [InlineData(int.MinValue, 0)]
        public void WaitToCpm_WaitInMs_CpmAsInt(int wait, int word_length)
        {
            var result = ReadingTextHelper.WaitToCpm(wait, word_length);
            Assert.IsType<int>(result);
        }

        [Theory]
        [InlineData(300, 200)]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        [InlineData(int.MinValue, 0)]
        public void WpmToWaitMs_Wpm_WaitInMs(int wpm, int expetedWait)
        {
            var result = ReadingTextHelper.WpmToWaitMs(wpm);
            Assert.Equal(expetedWait, result);
        }

        [Theory]
        [InlineData(300)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void WpmToWaitMs_Wpm_WaitAsInt(int wpm)
        {
            var result = ReadingTextHelper.WpmToWaitMs(wpm);
            Assert.IsType<int>(result);
        }

        [Theory]
        [InlineData(1800, 6, 200)]
        [InlineData(0, 6, 0)]
        [InlineData(200, 0, 0)]
        [InlineData(int.MinValue, 0, 0)]
        public void CpmToWaitMs_Cpm_WaitInMs(int cpm, int word_length, int expetedWait)
        {
            var result = ReadingTextHelper.CpmToWaitMs(cpm, word_length);
            Assert.Equal(expetedWait, result);
        }

        [Theory]
        [InlineData(1800, 6)]
        [InlineData(0, 6)]
        [InlineData(200, 0)]
        [InlineData(int.MinValue, 0)]
        public void CpmToWaitMs_Cpm_WaitAsInt(int cpm, int word_length)
        {
            var result = ReadingTextHelper.CpmToWaitMs(cpm, word_length);
            Assert.IsType<int>(result);
        }

        [Theory]
        [InlineData(100, 60000, 100)]
        [InlineData(100, 0, 0)]
        [InlineData(int.MinValue, 0, 0)]
        [InlineData(200, 0, 0)]
        public void GetSpeedIncreaseContant_ProperData_SpeedIncrease(double accPerMin,
            long msPassed, double expeted)
        {
            var result = ReadingTextHelper.GetSpeedIncrease(accPerMin, msPassed);
            Assert.Equal(expeted, result);
        }

        [Theory]
        [InlineData(100, 200, 60000, 30000, 50)]
        [InlineData(200, 200, 60000, long.MaxValue, 0)]
        [InlineData(100, 200, 60000, long.MaxValue, 100)]
        [InlineData(100, 200, 60000, 0, 0)]
        public void GetSpeedIncreaseInitial_ProperData_SpeedIncrease(double initialSpeed,
            double targetSpeed, long accTimeMs, long msPassed, double expeted)
        {
            var result = ReadingTextHelper
                .GetSpeedIncrease(initialSpeed, targetSpeed, accTimeMs, msPassed);
            Assert.Equal(expeted, result);
        }

        [Theory]
        [InlineData("a b c d e f", 6)]
        [InlineData("a     d    ", 2)]
        [InlineData("  a   d    ", 2)]
        [InlineData("           ", 0)]
        [InlineData("", 0)]
        [InlineData(null, 0)]
        public void TextWordsAmt_Text_WordAmt(string text, int expected)
        {
            var result = ReadingTextHelper
                .TextWordsAmt(text);
            Assert.Equal(expected, result);
        }

        public static TheoryData<string, ReadingWord[]> TextToArray_Text_WordArray_Data =>
            new TheoryData<string, ReadingWord[]>
            {
                {
                    "a b c",
                    new ReadingWord[]
                    {
                        new ReadingWord { StartLocation = 0, EndLocation = 1 },
                        new ReadingWord { StartLocation = 2, EndLocation = 3 },
                        new ReadingWord { StartLocation = 4, EndLocation = 5 }
                    }
                },
                {
                    "a     d    ",
                    new ReadingWord[]
                    {
                        new ReadingWord { StartLocation = 0, EndLocation = 1 },
                        new ReadingWord { StartLocation = 6, EndLocation = 7 }
                    }
                },
                {
                    "  a   d    ",
                    new ReadingWord[]
                    {
                        new ReadingWord { StartLocation = 2, EndLocation = 3 },
                        new ReadingWord { StartLocation = 6, EndLocation = 7 }
                    }
                },
                { "           ", new ReadingWord[] { } },
                { "", new ReadingWord[] { } },
                { null, new ReadingWord[] { } }
            };

        [Theory]
        [MemberData(nameof(TextToArray_Text_WordArray_Data))]
        public void TextToArray_Text_WordArray(string text, ReadingWord[] expected)
        {
            var result = ReadingTextHelper
                .TextToArray(text)
                .ToList();

            Assert.Equal(expected.Length, result.Count);
            for (int i = 0; i < expected.Length; ++i)
            {
                Assert.Equal(expected[i].StartLocation, result[i].StartLocation);
                Assert.Equal(expected[i].EndLocation, result[i].EndLocation);
            }
        }

        public static TheoryData<ReadingSession, List<ReadingWord>, ReadingSpeedGraphSet> GetReadingSpeedGraphSet_TextArray_GraphDataSet_Data =>
            new TheoryData<ReadingSession, List<ReadingWord>, ReadingSpeedGraphSet>
            {
                {
                    new ReadingSession
                    {
                        //Thu Jan 14 2021 22:39:42 GMT+0100 (Central European Standard Time)
                        StartTime = new DateTime(2021, 1, 14, 22, 39, 42),
                        //Thu Jan 14 2021 22:39:43 GMT+0100 (Central European Standard Time)
                        EndTime = new DateTime(2021, 1, 14, 22, 39, 43),
                        StartLocation = 0,
                        EndLocation = 3,
                        OptionsLog = new OptionsLog
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
                    }, 
                    new List<ReadingWord>
                    {
                        new ReadingWord { StartLocation = 0, EndLocation = 6 },
                        new ReadingWord { StartLocation = 7, EndLocation = 15 },
                        new ReadingWord { StartLocation = 16, EndLocation = 21 },
                        new ReadingWord { StartLocation = 22, EndLocation = 27 },
                        new ReadingWord { StartLocation = 28, EndLocation = 37 },
                        new ReadingWord { StartLocation = 38, EndLocation = 43 },
                        new ReadingWord { StartLocation = 44, EndLocation = 46 }
                    }, 
                    new ReadingSpeedGraphSet
                    {
                        Start = new DateTime(2021, 1, 14, 22, 39, 42), 
                        End = new DateTime(2021, 1, 14, 22, 39, 43),
                        Points = new List<ReadingSpeedGraphPoint> 
                        {
                            new ReadingSpeedGraphPoint { Cpm = 1800, MsFromStart = 0, WordStart = 0, Wpm = 300 },
                            new ReadingSpeedGraphPoint { Cpm = 2400, MsFromStart = 200, WordStart = 7, Wpm = 300.333 },
                            new ReadingSpeedGraphPoint { Cpm = 1500, MsFromStart = 400, WordStart = 16, Wpm = 300.667 }
                        },
                        SpeedType = "WPM"
                    }
                }
            };

        [Theory]
        [MemberData(nameof(GetReadingSpeedGraphSet_TextArray_GraphDataSet_Data))]
        public void GetReadingSpeedGraphSet_SessionAndTextArray_GraphDataSet(ReadingSession readingSession, List<ReadingWord> textArray, ReadingSpeedGraphSet expected)
        {
            var result = ReadingTextHelper
                .GetReadingSpeedGraphSet(readingSession, textArray);

            Assert.Equal(expected.Start, result.Start);
            Assert.Equal(expected.End, result.End);
            Assert.Equal(expected.SpeedType, result.SpeedType);
            var expectedPoints = expected.Points.ToList();
            var resultPoints = expected.Points.ToList();
            Assert.Equal(expectedPoints.Count, resultPoints.Count);
            for (int i = 0; i < expectedPoints.Count; ++i)
            {
                Assert.Equal(expectedPoints[i].Cpm, resultPoints[i].Cpm, 2);
                Assert.Equal(expectedPoints[i].Wpm, resultPoints[i].Wpm, 2);
                Assert.Equal(expectedPoints[i].MsFromStart, resultPoints[i].MsFromStart);
                Assert.Equal(expectedPoints[i].WordStart, resultPoints[i].WordStart);
            }
        }

        public static TheoryData<IEnumerable<ReadingSession>, IEnumerable<ReadingWord>, ReadingSpeedGraphData> GetReadingSpeedGraphData_SessionAndTextArray_GraphData_Data =>
            new TheoryData<IEnumerable<ReadingSession>, IEnumerable<ReadingWord>, ReadingSpeedGraphData>
            {
                {
                    new List<ReadingSession>
                    {
                        new ReadingSession
                        {
                            //Thu Jan 14 2021 22:39:42 GMT+0100 (Central European Standard Time)
                            StartTime = new DateTime(2021, 1, 14, 22, 39, 42),
                            //Thu Jan 14 2021 22:39:43 GMT+0100 (Central European Standard Time)
                            EndTime = new DateTime(2021, 1, 14, 22, 39, 43),
                            StartLocation = 0,
                            EndLocation = 3,
                            OptionsLog = new OptionsLog
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
                        }
                    },
                    new List<ReadingWord>
                    {
                        new ReadingWord { StartLocation = 0, EndLocation = 6 },
                        new ReadingWord { StartLocation = 7, EndLocation = 15 },
                        new ReadingWord { StartLocation = 16, EndLocation = 21 },
                        new ReadingWord { StartLocation = 22, EndLocation = 27 },
                        new ReadingWord { StartLocation = 28, EndLocation = 37 },
                        new ReadingWord { StartLocation = 38, EndLocation = 43 },
                        new ReadingWord { StartLocation = 44, EndLocation = 46 }
                    },
                    new ReadingSpeedGraphData
                    {
                        Empty = false, 
                        AllCharactersCount = 46,
                        Sets = new List<ReadingSpeedGraphSet>
                        {
                            new ReadingSpeedGraphSet
                            {
                                Start = new DateTime(2021, 1, 14, 22, 39, 42),
                                End = new DateTime(2021, 1, 14, 22, 39, 43),
                                Points = new List<ReadingSpeedGraphPoint>
                                {
                                    new ReadingSpeedGraphPoint { Cpm = 1800, MsFromStart = 0, WordStart = 0, Wpm = 300 },
                                    new ReadingSpeedGraphPoint { Cpm = 2400, MsFromStart = 200, WordStart = 7, Wpm = 300.333 },
                                    new ReadingSpeedGraphPoint { Cpm = 1500, MsFromStart = 400, WordStart = 16, Wpm = 300.667 }
                                },
                                SpeedType = "WPM"
                            }
                        }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(GetReadingSpeedGraphData_SessionAndTextArray_GraphData_Data))]
        public void GetReadingSpeedGraphData_SessionAndTextArray_GraphData(IEnumerable<ReadingSession> readingSessions, IEnumerable<ReadingWord> textArray, ReadingSpeedGraphData expected)
        {
            var result = ReadingTextHelper
                .GetReadingSpeedGraphData(readingSessions, textArray);

            Assert.Equal(expected.AllCharactersCount, result.AllCharactersCount);
            Assert.Equal(expected.Empty, result.Empty);
            Assert.Equal(expected.Sets.Count(), expected.Sets.Count());
            for (int i = 0; i < expected.Sets.Count(); ++i)
            {
                var expectedSet = expected.Sets.ElementAt(i);
                var resultSet = result.Sets.ElementAt(i);
                Assert.Equal(expectedSet.Start, resultSet.Start);
                Assert.Equal(expectedSet.End, resultSet.End);
                Assert.Equal(expectedSet.SpeedType, resultSet.SpeedType);
                var expectedPoints = expectedSet.Points.ToList();
                var resultPoints = expectedSet.Points.ToList();
                Assert.Equal(expectedPoints.Count, resultPoints.Count);
                for (int j = 0; j < expectedPoints.Count ; ++j)
                {
                    Assert.Equal(expectedPoints[j].Cpm, resultPoints[j].Cpm, 2);
                    Assert.Equal(expectedPoints[j].Wpm, resultPoints[j].Wpm, 2);
                    Assert.Equal(expectedPoints[j].MsFromStart, resultPoints[j].MsFromStart);
                    Assert.Equal(expectedPoints[j].WordStart, resultPoints[j].WordStart);
                }
            } 
        }


        public static TheoryData<IEnumerable<ReadingSession>, string, ReadingSpeedGraphData> GetReadingSpeedGraphData_SessionAndText_GraphData_Data =>
            new TheoryData<IEnumerable<ReadingSession>, string, ReadingSpeedGraphData>
            {
                {
                    new List<ReadingSession>
                    {
                        new ReadingSession
                        {
                            //Thu Jan 14 2021 22:39:42 GMT+0100 (Central European Standard Time)
                            StartTime = new DateTime(2021, 1, 14, 22, 39, 42),
                            //Thu Jan 14 2021 22:39:43 GMT+0100 (Central European Standard Time)
                            EndTime = new DateTime(2021, 1, 14, 22, 39, 43),
                            StartLocation = 0,
                            EndLocation = 3,
                            OptionsLog = new OptionsLog
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
                        }
                    },
                    "Johnny Osbourne (born Errol Osbourne, 1948) is ",
                    new ReadingSpeedGraphData
                    {
                        Empty = false,
                        AllCharactersCount = 46,
                        Sets = new List<ReadingSpeedGraphSet>
                        {
                            new ReadingSpeedGraphSet
                            {
                                Start = new DateTime(2021, 1, 14, 22, 39, 42),
                                End = new DateTime(2021, 1, 14, 22, 39, 43),
                                Points = new List<ReadingSpeedGraphPoint>
                                {
                                    new ReadingSpeedGraphPoint { Cpm = 1800, MsFromStart = 0, WordStart = 0, Wpm = 300 },
                                    new ReadingSpeedGraphPoint { Cpm = 2400, MsFromStart = 200, WordStart = 7, Wpm = 300.333 },
                                    new ReadingSpeedGraphPoint { Cpm = 1500, MsFromStart = 400, WordStart = 16, Wpm = 300.667 }
                                },
                                SpeedType = "WPM"
                            }
                        }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(GetReadingSpeedGraphData_SessionAndText_GraphData_Data))]
        public void GetReadingSpeedGraphData_SessionAndText_GraphData(IEnumerable<ReadingSession> readingSessions, string text, ReadingSpeedGraphData expected)
        {
            var result = ReadingTextHelper
                .GetReadingSpeedGraphData(readingSessions, text);

            Assert.Equal(expected.AllCharactersCount, result.AllCharactersCount);
            Assert.Equal(expected.Empty, result.Empty);
            Assert.Equal(expected.Sets.Count(), expected.Sets.Count());
            for (int i = 0; i < expected.Sets.Count(); ++i)
            {
                var expectedSet = expected.Sets.ElementAt(i);
                var resultSet = result.Sets.ElementAt(i);
                Assert.Equal(expectedSet.Start, resultSet.Start);
                Assert.Equal(expectedSet.End, resultSet.End);
                Assert.Equal(expectedSet.SpeedType, resultSet.SpeedType);
                var expectedPoints = expectedSet.Points.ToList();
                var resultPoints = expectedSet.Points.ToList();
                Assert.Equal(expectedPoints.Count, resultPoints.Count);
                for (int j = 0; j < expectedPoints.Count; ++j)
                {
                    Assert.Equal(expectedPoints[j].Cpm, resultPoints[j].Cpm, 2);
                    Assert.Equal(expectedPoints[j].Wpm, resultPoints[j].Wpm, 2);
                    Assert.Equal(expectedPoints[j].MsFromStart, resultPoints[j].MsFromStart);
                    Assert.Equal(expectedPoints[j].WordStart, resultPoints[j].WordStart);
                }
            }
        }



    }
}
