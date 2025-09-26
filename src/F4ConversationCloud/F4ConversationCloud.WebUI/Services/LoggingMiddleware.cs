using F4ConversationCloud.Application.Common.Interfaces.Services;
using System.Text;

namespace F4ConversationCloud.WebUI.Services
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly HashSet<string> _excludedPaths = new HashSet<string>
        {
            "/swagger/index.html",
            "/swagger/index.js",
            "/swagger/v0/swagger.json"
        };
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAPILogService _logger)
        {
            var requestPath = context.Request.Path.Value;

            // Skip logging for excluded paths
            if (_excludedPaths.Contains(requestPath))
            {
                await _next(context);
                return;
            }

            var content = new StringBuilder();
            string fileName = string.Empty;

            content.AppendLine("[" + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dddd, dd MMMM yyyy hh:mm tt") + "]\n");

            string request = await LogRequest(context);

            content.Append(request);

            var originalResponseBody = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                string response = string.Empty;
                try
                {
                    await _next.Invoke(context);

                    response = await LogResponse(context, responseBody, originalResponseBody);
                }
                catch (Exception ex)
                {
                    response = await LogException(context, ex);
                }

                content.AppendLine("\n");
                content.Append(response);

                foreach (var item in context.Request.Path.ToString().Split('/'))
                {
                    if (string.IsNullOrEmpty(fileName))
                        fileName = item;
                    else
                        fileName = fileName + "-" + item;
                }

                await _logger.CreateLog(fileName, content.ToString());
            }
        }

        private async Task<string> LogRequest(HttpContext context)
        {
            var requestContent = new StringBuilder();

            requestContent.AppendLine("=== Request Info ===");
            requestContent.AppendLine($"method = {context.Request.Method.ToUpper()}");
            requestContent.AppendLine($"path = {context.Request.Path}");
            requestContent.AppendLine($"host = {context.Request.Host}");
            requestContent.AppendLine($"clientIp = {context.Connection.RemoteIpAddress}");

            requestContent.AppendLine("-- headers --");
            foreach (var (headerKey, headerValue) in context.Request.Headers)
            {
                requestContent.AppendLine($"header = {headerKey}    value = {headerValue}");
            }

            requestContent.AppendLine("-- Query --");
            foreach (var (key, value) in context.Request.Query)
            {
                requestContent.AppendLine($"Parameter = {key}    value = {value}");
            }

            requestContent.AppendLine("-- body --");

            context.Request.EnableBuffering();

            var requestReader = new StreamReader(context.Request.Body);
            var content = await requestReader.ReadToEndAsync();
            requestContent.AppendLine($"body = {content}");


            context.Request.Body.Position = 0;

            return requestContent.ToString();
        }

        private async Task<string> LogResponse(HttpContext context, MemoryStream responseBody, Stream originalResponseBody)
        {
            var responseContent = new StringBuilder();

            responseContent.AppendLine("=== Response Info ===");
            responseContent.AppendLine($"statusCode = {context.Response.StatusCode}");
            responseContent.AppendLine($"contentType = {context.Response.ContentType}");

            responseContent.AppendLine("-- headers --");
            foreach (var (headerKey, headerValue) in context.Response.Headers)
            {
                responseContent.AppendLine($"header = {headerKey}    value = {headerValue}");
            }

            responseContent.AppendLine("-- body --");
            bool IsDocumentType = context.Response.ContentType?.Contains("image", StringComparison.CurrentCultureIgnoreCase) ?? false;
            responseBody.Position = 0;
            if (!IsDocumentType)
            {
                var content = await new StreamReader(responseBody).ReadToEndAsync();
                responseContent.AppendLine($"body = {content}");
            }
            responseBody.Position = 0;
            await responseBody.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;

            return responseContent.ToString();
        }

        private async Task<string> LogException(HttpContext context, Exception ex)
        {
            var exceptionContent = new StringBuilder();

            exceptionContent.AppendLine("=== Response Info ===");
            exceptionContent.AppendLine($"statusCode = {context.Response.StatusCode}");
            exceptionContent.AppendLine($"contentType = {context.Response.ContentType}");

            exceptionContent.AppendLine("-- headers --");
            foreach (var (headerKey, headerValue) in context.Response.Headers)
            {
                exceptionContent.AppendLine($"header = {headerKey}    value = {headerValue}");
            }

            exceptionContent.AppendLine("-- exception --");
            exceptionContent.AppendLine($"message = {ex.Message}");
            exceptionContent.AppendLine($"stacktrace = {ex.StackTrace}");

            return exceptionContent.ToString();
        }
    }
}
