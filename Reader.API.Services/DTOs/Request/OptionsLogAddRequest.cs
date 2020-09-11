using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Request
{
    public class OptionsLogAddRequest
    {
        public int InitialWPM { get; set; }
        public int InitialCPM { get; set; }
        public int TargetWPM { get; set; }
        public int TargetCPM { get; set; }
        public int BreakIfLonger { get; set; }
        public int SlowIfLonger { get; set; }
        public int AppendIfShorter { get; set; }
        public int MaxAppend { get; set; }
        public int InitialAccelaretionTimeSecs { get; set; }
        public int SlowTo { get; set; }
        public double AddPerMin { get; set; }
    }
}
