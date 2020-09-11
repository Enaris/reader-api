using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public interface IUploadService
    {
        Task<string> UploadImage(IFormFile request, Guid aspUserId);
        Task<IEnumerable<string>> UploadImages(IEnumerable<IFormFile> files, Guid aspUserId);
    }
}