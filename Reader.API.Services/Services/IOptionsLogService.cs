using Reader.API.Services.DTOs.Request;
using Reader.API.Services.DTOs.Response;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public interface IOptionsLogService
    {
        Task<OptionsLogDetails> CreateOptionsLog(OptionsLogAddRequest request);
    }
}