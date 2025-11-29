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

        public async Task<string> SaveFileFromBase64Async(string base64String)
        {
            try
            {
                var fullPath = "";
                var FileType = base64String.Split(':')[1].Split(';')[0].Split('/')[0].ToLower();
                if (!string.IsNullOrEmpty(base64String))
                {
                    string folderName = "Other";
                    
                    if (FileType == "image")
                    {
                         folderName = "TemplateImage";
                    }
                    

                    if ((base64String.StartsWith("data:image", StringComparison.OrdinalIgnoreCase) || base64String.StartsWith("data:application/pdf", StringComparison.OrdinalIgnoreCase) ) && base64String.Contains("base64,"))
                    {
                        base64String = base64String.Substring(base64String.IndexOf("base64,") + 7);
                    }

                    byte[] fileBytes = Convert.FromBase64String(base64String);

                    string fileExtension = GetFileExtension(fileBytes);

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString();
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName + fileExtension);

                    await File.WriteAllBytesAsync(filePath, fileBytes);

                    var relativePath = "/" + folderName + "/" + uniqueFileName + fileExtension;

                    var httpContext = _httpContextAccessor.HttpContext;
                    fullPath = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{relativePath}";

                    return fullPath;
                }

                return fullPath;
            }
            catch (Exception ex) 
            {
                return string.Empty; 
            }
        }

        private string GetFileExtension(byte[] fileBytes)
        {
            if (fileBytes.Length > 8 &&
                fileBytes[0] == 0x89 && fileBytes[1] == 0x50 && fileBytes[2] == 0x4E &&
                fileBytes[3] == 0x47)
                return ".png";

            if (fileBytes.Length > 3 &&
                fileBytes[0] == 0xFF && fileBytes[1] == 0xD8)
                return ".jpg";

            if (fileBytes.Length > 3 &&
                fileBytes[0] == 0x47 && fileBytes[1] == 0x49 &&
                fileBytes[2] == 0x46)
                return ".gif";

            if (fileBytes.Length > 4 &&
                fileBytes[0] == 0x25 && fileBytes[1] == 0x50 &&
                fileBytes[2] == 0x44 && fileBytes[3] == 0x46)
                return ".pdf";

            return ".bin";
        }

        public async Task<bool> DeleteFileFromRoot(string folderName, string fileUrl)
        {
            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
                var oldFilePath = Path.Combine(uploadsFolder, Path.GetFileName(fileUrl));

                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<byte[]> GetFileAsync(string folderName, string fileName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", fileName);

            return await File.ReadAllBytesAsync(filePath);
        }
    }
}

