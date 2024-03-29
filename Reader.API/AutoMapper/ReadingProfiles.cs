﻿using AutoMapper;
using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response;
using Reader.API.Services.DTOs.Response.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reader.API.AutoMapper
{
    public class ReadingProfiles : Profile
    {
        public ReadingProfiles()
        {
            CreateMap<ReadingAddRequest, Reading>();
            CreateMap<Reading, ReadingListItem>()
                .ForMember(rli => rli.Tags, o => o.MapFrom(r => r.ReadingTags));
            CreateMap<Reading, ReadingDetails>()
                .ForMember(rd => rd.Tags, o => o.MapFrom(r => r.ReadingTags));
            CreateMap<ReadingUpdateRequest, Reading>()
                .ForMember(r => r.Text, o => o.PreCondition(rur => rur.ChangeText));
            CreateMap<Reading, ReadingNameAndId>();
        }
    }
}
