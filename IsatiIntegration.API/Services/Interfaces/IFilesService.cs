using IsatiIntegration.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services.Interfaces
{
    public interface IFilesService
    {
        Task<string> UploadFile(string filename, byte[] fileBytes, bool shouldOverride = true);

        Task<FileModel> GetFileById(string id);
        Task<FileModel> GetFileByName(string name);
    }
}
