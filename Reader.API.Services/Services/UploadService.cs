using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.API.Services.Services
{
    public class UploadService : IUploadService
    {
        const string FOLDER_PATH = "Static/Images/Covers";

        public async Task<string> UploadImage(IFormFile request, Guid aspUserId)
        {
            if (request.Length <= 0)
            {
                return null;
            }

            CreateUserFolderIfDoesNotExist(aspUserId);
            var filePath = GetFilePath(Path.GetExtension(request.FileName), aspUserId);

            using (var stream = System.IO.File.Create(filePath))
            {
                await request.CopyToAsync(stream);
            }

            return filePath;
        }

        public async Task<IEnumerable<string>> UploadImages(IEnumerable<IFormFile> files, Guid aspUserId)
        {
            var result = new List<string>(files.Count());
            foreach (var f in files)
            {
                var uploadedImage = await UploadImage(f, aspUserId);
                if (uploadedImage == null)
                    continue;

                result.Add(uploadedImage);
            }

            return result;
        }

        private string GetFilePath(string extension, Guid aspUserId)
        {
            var folderPath = FOLDER_PATH;
            return $"{folderPath}/{aspUserId}/{Guid.NewGuid()}{extension}";
        }

        private void CreateUserFolderIfDoesNotExist(Guid aspUserId)
        {
            var userFolderPath = $"{FOLDER_PATH}/{aspUserId}";
            if (!Directory.Exists(userFolderPath))
                Directory.CreateDirectory(userFolderPath);
        }
    }
}
