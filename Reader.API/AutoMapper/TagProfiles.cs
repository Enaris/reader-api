﻿using AutoMapper;
using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reader.API.AutoMapper
{
    public class TagProfiles : Profile
    {
        public TagProfiles()
        {
            CreateMap<Tag, TagDto>();
            CreateMap<TagAddRequest, Tag>();

            CreateMap<ReadingTag, TagDto>()
                .ForMember(t => t.Id, o => o.MapFrom(rt => rt.Tag.Id))
                .ForMember(t => t.Name, o => o.MapFrom(rt => rt.Tag.Name));

            CreateMap<Tag, TagTableItem>()
                .ForMember(t => t.TagedCount, o => o.MapFrom(t => t.ReadingTags.Count));
            CreateMap<Tag, TagDetails>()
                .ForMember(t => t.TagedCount, o => o.MapFrom(t => t.ReadingTags.Count));
        }
    }
}
