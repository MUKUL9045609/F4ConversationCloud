using F4ConversationCloud.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class FileUploadService : IFileUploadService
    {
        
        private readonly string FolderRoot = "wwwroot/";
        private IHttpContextAccessor _httpContextAccessor { get; set; }

        public FileUploadService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CheckFileExist(string folderPath, string fileName)
        {
            try
            {
                string newFolder = folderPath;
                if (fileName.Contains("/"))
                {
                    newFolder += "/" + string.Join("/", fileName.Split("/").Take(fileName.Split("/").Count() - 1));
                }
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), FolderRoot, newFolder)))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), FolderRoot, newFolder));
                }
                string FilePaths = Path.Combine(Directory.GetCurrentDirectory(), FolderRoot, folderPath, fileName);
                return File.Exists(FilePaths);
            }
            catch (Exception ex)
            {

            }
            return await Task.FromResult(false);
        }

        public async Task<Stream> GetFile(string blobName)
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), FolderRoot, blobName);
                return File.OpenRead(path);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> UploadFile(Stream fileStream, string filePath, string contentType)
        {
            string blobName = $"{filePath}";

            try
            {
                string FullFilePath = Path.Combine(Directory.GetCurrentDirectory(), FolderRoot, filePath);
                if (fileStream.CanSeek)
                    fileStream.Position = 0;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    using (FileStream fs = new FileStream(FullFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            memoryStream.WriteTo(fs);
                        }
                    }

                    return GenerateUrl(FullFilePath);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GenerateUrl(string blobUrl)
        {
            string finalblobUrl = string.Empty;
            try
            {
                string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                finalblobUrl = blobUrl.Replace(FolderPath, GetFilePaths());

                return finalblobUrl;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public string GetFilePaths()
        {
            try
            {
                var httpRequest = _httpContextAccessor.HttpContext.Request;

                var scheme = httpRequest.Scheme;  // Returns "http" or "https"
                                                  // Get the domain (hostname)
                var domain = httpRequest.Host.Host;  // Returns "www.test.com"
                                                     // Optionally, you can also get the port if it's specified
                var port = httpRequest.Host.Port.HasValue ? $":{httpRequest.Host.Port}" : string.Empty;


                var fullUrl = $"{scheme}://{domain}{port}";

                return fullUrl;
            }

            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    }
}

