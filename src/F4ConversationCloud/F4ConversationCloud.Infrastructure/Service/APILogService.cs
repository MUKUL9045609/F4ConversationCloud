using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Helpers;
using F4ConversationCloud.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using Twilio.TwiML;


namespace F4ConversationCloud.Infrastructure.Service
{
    public class APILogService : IAPILogService
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly string FolderName = "Logs";
        private readonly IAPILogRepository _repository;

        public APILogService(IFileUploadService fileUploadService, IAPILogRepository repository)
        {
            _fileUploadService = fileUploadService;
            _repository = repository;
        }
        public async Task GenerateLog(APILogModel aPILogModel)
        {
            try
            {
                var content = new StringBuilder();

                content.AppendLine("[" + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dddd, dd MMMM yyyy hh:mm tt") + "]\n");

                content.AppendLine("=== Request Info ===");
                content.AppendLine($"method = {aPILogModel.MethodType}");
                content.AppendLine($"path = {aPILogModel.APIUrl}");

                content.AppendLine("-- headers --");
                foreach (var (headerKey, headerValue) in aPILogModel.Headers)
                {
                    content.AppendLine($"header = {headerKey}    value = {headerValue}");
                }

                content.AppendLine($"query = {aPILogModel.Query}");
                content.AppendLine("-- body --");

                content.AppendLine($"body = {aPILogModel.Request}");

                content.AppendLine("\n");

                content.AppendLine("=== Response Info ===");

                content.AppendLine("-- body --");

                content.AppendLine($"body = {aPILogModel.Response}");

                content.AppendLine($"error = {aPILogModel.Error}");
                content.AppendLine($"stackTrace = {aPILogModel.StackTrace}");

                await CreateLog(aPILogModel.Name, content.ToString());
            }
            catch
            {

            }
        }

        public async Task CreateLog(string fileName, string newContent)
        {
            try
            {
                string content = newContent;
                UnicodeEncoding uniEncoding = new UnicodeEncoding();

                var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dd MMMM yyyy");
                fileName = fileName + " [" + currentTime + "]" + ".txt";

                bool exists = await _fileUploadService.CheckFileExist(FolderName, currentTime + "/" + fileName);

                if (exists)
                {
                    using (Stream existing = await _fileUploadService.GetFile(this.FolderName + "/" + currentTime + "/" + fileName))
                    {
                        StreamReader reader = new StreamReader(existing, Encoding.Unicode);
                        string oldContent = reader.ReadToEnd();

                        content = oldContent + "\n\n" + newContent;
                    }
                }

                byte[] memstring = uniEncoding.GetBytes(content);

                using (MemoryStream memStream = new MemoryStream())
                {
                    memStream.Write(memstring, 0, memstring.Length);

                    await _fileUploadService.UploadFile(memStream, this.FolderName + "/" + currentTime + "/" + fileName, "text/plain");
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<T> CallExternalAPI<T>(string EndPoint, string MethodType, T RequestBody, Dictionary<string, string> OAUths, string APIType, Dictionary<string, string> QuerryStringParameters, bool AddLogs = true, bool IsExternalAPI = false, string BodyType = "json")
        {
            var ReturnResponse = (T)Activator.CreateInstance(typeof(T));
            APIRequestModel<T> model = new APIRequestModel<T>();
            bool isSuccess = false;
            string errorMessage = null;
            string rawResponse = null;
            int statusCode = 0;
            try
            {
                model = new APIRequestModel<T>
                {
                    OAuths = OAUths,
                    APIType = APIType,
                    EndPoint = EndPoint,
                    QuerryStringParameters = QuerryStringParameters,
                    RequestBody = RequestBody,
                    RequestType = MethodType,
                    AddLogs = AddLogs,
                    IsExternalAPI = IsExternalAPI,
                    BodyType = BodyType
                };

                var response = GenericAPIHelper.CallAPIGeneric<T>(model);

                isSuccess = response.Status;
                errorMessage = response.ErrorMessage;
                rawResponse = await response.Response.Content.ReadAsStringAsync();
                statusCode = (int)response.Response.StatusCode;
                
                ReturnResponse = JsonConvert.DeserializeObject<T>(rawResponse);
                
            }
            catch (Exception ex)
            {
                isSuccess = false;
                errorMessage = ex.Message;
            }
            finally
            {
                int insertedId = await _repository.SaveApiResponseAsync(isSuccess: isSuccess,errorMessage: errorMessage,rawResponse: rawResponse,statusCode: statusCode);
            }
            return ReturnResponse;
        }
    }
}
