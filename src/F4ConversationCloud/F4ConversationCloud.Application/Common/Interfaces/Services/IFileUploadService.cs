using System;
using System.Collections.Generic;
using System.IO;
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
        Task<string> SaveFileFromBase64Async(string base64String);
        Task<bool> DeleteFileFromRoot(string folderName, string fileUrl);
        Task<byte[]> GetFileAsync(string folderName, string fileName);


    }

}
