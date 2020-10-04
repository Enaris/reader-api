using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.Helpers.ReadingText
{
    public class ReadingWord
    {
        public int StartLocation { get; set; }
        public int EndLocation { get; set; }

        public int WordLength => EndLocation - StartLocation;
    }
}
