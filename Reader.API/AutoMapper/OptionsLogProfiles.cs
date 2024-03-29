﻿using AutoMapper;
using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reader.API.AutoMapper
{
    public class OptionsLogProfiles : Profile
    {
        public OptionsLogProfiles()
        {
            CreateMap<OptionsLogAddRequest, OptionsLog>();
            CreateMap<OptionsLog, OptionsLogDetails>();
        }
    }
}
