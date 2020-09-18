using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Reader.API.Services.Services
{
    public class FileDeleteService : IFileDeleteService
    {
        public FileDeleteResult DeleteFile(string url)
        {
            if (!File.Exists(url))
                return FileDeleteResult.FileDoesNotExist;
            try
            {
                File.Delete(url);
            }
            catch (Exception)
            {
                return FileDeleteResult.OtherError;
            }

            return FileDeleteResult.Deleted;
        }
    }

    public enum FileDeleteResult
    {
        FileDoesNotExist, 
        OtherError, 
        Deleted
    }
}
