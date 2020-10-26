using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.DTOs.Response.Tag
{
    public class TagTableItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TagedCount { get; set; }
        public double MeanWpm { get; set; }
        public double MeanCpm { get; set; }
    }
}
