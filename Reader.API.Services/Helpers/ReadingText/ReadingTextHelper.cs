using Reader.API.Services.Helpers.ReadingText;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Reader.API.Services.DTOs.Response.ReadingSpeedGraph;
using Reader.API.DataAccess.DbModels;

namespace Reader.API.Services.Helpers
{
    public static class ReadingTextHelper
    {
        public static IEnumerable<ReadingWord> TextToArray(string readingText)
        {
            var result = new List<ReadingWord>();
            int wordStart = 0;
            int wordEnd = 0;
            var text = readingText.ToList();

            while (wordEnd < readingText.Length)
            {
                wordStart = text.FindIndex(wordEnd, c => !char.IsWhiteSpace(c));
                if (wordStart == -1)
                    break;

                wordEnd = text.FindIndex(wordStart, c => char.IsWhiteSpace(c));
                if (wordEnd == -1)
                    wordEnd = text.Count;
                result.Add(new ReadingWord { StartLocation = wordStart, EndLocation = wordEnd });
            }
            return result;
        }

        public static ReadingSpeedGraphData GetReadingSpeedGraphData(IEnumerable<ReadingSession> readingSessions, string readingText)
        {
            var textArray = TextToArray(readingText).ToList();
            var result = new ReadingSpeedGraphData
            {
                AllCharactersCount = textArray.Last().EndLocation
            };
            var sets = new List<ReadingSpeedGraphSet>(readingSessions.Count());

            foreach (var rs in readingSessions)
            {
                sets.Add(GetReadingSpeedGraphSet(rs, textArray));
            }
            result.Sets = sets;
            return result;
        }

        public static ReadingSpeedGraphSet GetReadingSpeedGraphSet(ReadingSession readingSession, List<ReadingWord> textArray)
        {
            long msPassed = 0;
            var speedType = readingSession.OptionsLog.GetSpeedType();
            var initialSpeed = (double)readingSession.OptionsLog.GetInitialSpeed();
            var targetSpeed = (double)readingSession.OptionsLog.GetTargetSpeed();
            var doBreak = readingSession.OptionsLog.BreakIfLonger > 0;
            var doSlow = readingSession.OptionsLog.SlowIfLonger > 0;
            var doAppend = readingSession.OptionsLog.AppendIfShorter > 0;
            var doIniAcc = targetSpeed > 0;
            var doConstAcc = readingSession.OptionsLog.AddPerMin > 0;
            long iniAccTimeMs = readingSession.OptionsLog.InitialAccelerationTimeSecs * 1000;

            var result = new ReadingSpeedGraphSet 
            { 
                Start = readingSession.StartTime, 
                End = readingSession.EndTime,
                SpeedType = speedType 
            };
            var points = new List<ReadingSpeedGraphPoint>(readingSession.EndLocation - readingSession.StartLocation);

            var currentIndex = readingSession.StartLocation;
            var currentWord = textArray[currentIndex];
            var currentStart = currentWord.StartLocation;
            var currentEnd = currentWord.EndLocation;
            var currentLength = currentEnd - currentStart;
            var partLength = 0;
            var currentSpeed = initialSpeed;
            var appended = 0;
            // del

            while (currentIndex < readingSession.EndLocation && currentIndex < textArray.Count)
            {
                var didBreak = DidBreak(doBreak, currentWord.EndLocation, currentEnd);
                if (didBreak)
                {
                    currentStart = currentEnd;
                    currentEnd = currentWord.EndLocation;
                }

                currentLength = currentEnd - currentStart;
                partLength = currentLength;
                appended = 0;
                while (ShouldAppend(doAppend, 
                    appended, 
                    readingSession.OptionsLog.MaxAppend, 
                    readingSession.OptionsLog.AppendIfShorter, 
                    currentLength, 
                    doBreak, 
                    readingSession.OptionsLog.BreakIfLonger, 
                    partLength))
                {

                    ++currentIndex;
                    if (currentIndex >= textArray.Count)
                        break;
                    ++appended;
                    
                    currentWord = textArray[currentIndex];
                    currentStart = currentWord.StartLocation;
                    currentEnd = currentWord.EndLocation;
                    currentLength = currentWord.WordLength;
                    partLength += currentLength;
                }
                var shouldBreak = ShouldBreak(doBreak, readingSession.OptionsLog.BreakIfLonger, partLength);

                if (shouldBreak)
                {
                    var tooLongBy = partLength - readingSession.OptionsLog.BreakIfLonger;
                    currentEnd -= tooLongBy;
                    partLength -= tooLongBy;
                }

                var shouldSlow = ShouldSlow(doSlow, readingSession.OptionsLog.SlowIfLonger, partLength);
                var usedSpeed = shouldSlow ? readingSession.OptionsLog.SlowTo : currentSpeed;
                var wait = speedType == "WPM" ? WpmToWaitMs(usedSpeed) : CpmToWaitMs(usedSpeed, partLength);

                var point = new ReadingSpeedGraphPoint
                {
                    Wpm = speedType == "WPM" ? usedSpeed : WaitToWpm(wait),
                    Cpm = speedType == "CPM" ? usedSpeed : WaitToCpm(wait, partLength),
                    WordStart = currentStart,
                    MsFromStart = msPassed,
                };
                points.Add(point);

                msPassed += wait;

                if (!shouldBreak)
                {
                    ++currentIndex;
                    currentWord = textArray[currentIndex];
                    currentStart = currentWord.StartLocation;
                    currentEnd = currentWord.EndLocation;
                }

                if (ShouldDoIniAcc(doIniAcc, iniAccTimeMs, msPassed, currentSpeed, targetSpeed))
                {
                    var increase = GetSpeedIncrease(initialSpeed, targetSpeed, iniAccTimeMs, msPassed);
                    currentSpeed = initialSpeed + increase;
                }                
                else if (ShouldDoConstAcc(doConstAcc, msPassed, iniAccTimeMs, doIniAcc))
                {
                    var increase = GetSpeedIncrease(readingSession.OptionsLog.AddPerMin, wait);
                    currentSpeed += increase;
                }


            }
            result.Points = points;

            return result;
        }

        private static bool ShouldDoConstAcc(bool doConstAcc, long msPassed, long iniAccTimeMs, bool doIniAcc)
        {
            return (doConstAcc && !doIniAcc) ||
                (doConstAcc && doIniAcc && iniAccTimeMs < msPassed);
        }
        private static bool ShouldDoIniAcc(bool doIniAcc, long iniAccTime, long msPassed, double currentSpeed, double targetSpeed)
        {
            return doIniAcc && ((iniAccTime > msPassed) || (currentSpeed < targetSpeed));
        }
        private static bool ShouldSlow(bool doSlow, int slowIfLonger, int overallLength)
        {
            return doSlow && slowIfLonger < overallLength;
        }
        private static bool ShouldAppend(bool doAppend, int appended, int maxAppend, int appendIfShorter, int wordLength, bool doBreak, int breakIfLonger, int overallLength)
        {
            var append = appended < maxAppend;
            var appendShort = wordLength < appendIfShorter;
            var shortToBreak = breakIfLonger > overallLength;
            var doNotBreak = doBreak && shortToBreak;
            var allBreakConditions = !doBreak || doNotBreak;

            return doAppend && append && appendShort && allBreakConditions;
        }
        private static bool ShouldBreak(bool doBreak, int breakIfLonger, int overallLength)
        {
            return doBreak && breakIfLonger < overallLength;
        }
        private static bool DidBreak(bool doBreak, int wordEnd, int lastEnd)
        {
            return doBreak && wordEnd != lastEnd;
        }

        public static int WaitToWpm(int waitMs) => (int)Math.Floor(1.0 / (waitMs / 60000.0));
        public static int WaitToCpm(int waitMs, int len) => (int)Math.Floor(1.0 / ((double)waitMs / (double)len / 60000.0));
        public static int CpmToWaitMs(double cpm, int len) => (int)Math.Floor((60.0 / cpm * (double)len * 1000.0) + 0.5);
        public static int WpmToWaitMs(double wpm) => (int)Math.Floor((60.0 / wpm * 1000.0) + 0.5);
        public static double GetSpeedIncrease(double accPerMin, long msPassed) => (accPerMin / (60.0 * 1000.0)) * msPassed;
        public static double GetSpeedIncrease(double initialSpeed, double targetSpeed, long accTimeMs, long msPassed)
        {
            var maxIncrease = targetSpeed - initialSpeed;
            var result = (maxIncrease / accTimeMs) * msPassed;
            return result > maxIncrease ? maxIncrease : result;
        }
    }
}
