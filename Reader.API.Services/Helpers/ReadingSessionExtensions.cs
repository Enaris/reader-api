using Reader.API.DataAccess.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.Helpers
{
    public static class ReadingSessionExtensions
    {
        public static int GetWordsRead(this ReadingSession readingSession)
        {
            return readingSession.EndLocation - readingSession.StartLocation;
        } 
    }
}
