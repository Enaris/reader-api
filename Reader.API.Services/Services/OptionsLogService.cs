using AutoMapper;
using Reader.API.DataAccess.DbModels;
using Reader.API.DataAccess.Repositories;
using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public class OptionsLogService : IOptionsLogService
    {
        private readonly IOptionsLogRepository optionsLogRepo;
        private readonly IMapper mapper;

        public OptionsLogService(IOptionsLogRepository optionsLogRepo, IMapper mapper)
        {
            this.optionsLogRepo = optionsLogRepo;
            this.mapper = mapper;
        }

        public async Task<OptionsLogDetails> CreateOptionsLog(OptionsLogAddRequest request)
        {
            var optionsLogToAdd = mapper.Map<OptionsLog>(request);
            await optionsLogRepo.CreateAsync(optionsLogToAdd);
            await optionsLogRepo.SaveChangesAsync();
            return mapper.Map<OptionsLogDetails>(optionsLogToAdd);
        }
    }
}
