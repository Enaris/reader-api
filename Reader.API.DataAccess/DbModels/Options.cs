﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.DataAccess.DbModels
{
    public class Options
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int InitialWPM { get; set; }
        public int InitialCPM { get; set; }
        public int TargetWPM { get; set; }
        public int TargetCPM { get; set; }
        public int BreakIfLonger { get; set; }
        public int SlowIfLonger { get; set; }
        public int AppendIfShorter { get; set; }
        public int MaxAppend { get; set; }
        public int InitialAccelerationTimeSecs { get; set; }
        public int SlowTo { get; set; }
        public double AddPerMin { get; set; }

        public Guid ReaderUserId { get; set; }
        public ReaderUser ReaderUser { get; set; }
    }
}
