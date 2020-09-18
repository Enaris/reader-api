using AutoMapper;
using Reader.API.DataAccess.DbModels;
using Reader.API.Services.DTOs.Request;
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
        }
    }
}
