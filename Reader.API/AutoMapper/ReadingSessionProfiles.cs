using AutoMapper;
using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response.ReadingSession;
using Reader.API.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reader.API.AutoMapper
{
    public class ReadingSessionProfiles : Profile
    {
        public ReadingSessionProfiles()
        {
            CreateMap<SaveSessionRequest, ReadingSession>();
            CreateMap<ReadingSession, ReadingSessionDropDownItem>()
                .ForMember(d => d.SpeedType, o => o.MapFrom(s => s.OptionsLog.GetSpeedType()))
                .ForMember(d => d.WordsRead, o => o.MapFrom(s => s.GetWordsRead()));
        }
    }
}
