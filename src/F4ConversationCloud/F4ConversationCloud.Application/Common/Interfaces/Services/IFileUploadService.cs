using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface IFileUploadService
    {

        string GenerateUrl(string blobUrl);
        Task<bool> CheckFileExist(string folderPath, string fileName);
        Task<string> UploadFile(Stream fileStream, string filePath, string contentType);
        Task<Stream> GetFile(string blobName);
    }
}
