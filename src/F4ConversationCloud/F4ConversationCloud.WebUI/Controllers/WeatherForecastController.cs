using F4ConversationCloud.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace F4ConversationCloud.WebUI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ITemplateService _templateService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger , ITemplateService templateService)
        {
            _logger = logger;
            _templateService = templateService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("UploadFile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ConvertFileToBase64(IFormFile file)
        {
            try
            {

                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                if (!file.ContentType.StartsWith("image/"))
                    return BadRequest("Only image files are supported.");

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                string base64 = Convert.ToBase64String(fileBytes);
                string dataUri = $"data:{file.ContentType};base64,{base64}";

                return Ok(new
                {
                    file.FileName,
                    file.ContentType,
                    Base64Data = dataUri
                });
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }


        [HttpPost("UploadFileMeta")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFileMeta(IFormFile file)
        {
            try
            {

                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                if (!file.ContentType.StartsWith("image/"))
                    return BadRequest("Only image files are supported.");

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                string base64 = Convert.ToBase64String(fileBytes);
                string dataUri = $"data:{file.ContentType};base64,{base64}";

                string headerFileJsonString = await _templateService.UploadMetaImage(dataUri,"","");
                using JsonDocument doc = JsonDocument.Parse(headerFileJsonString);
                JsonElement root = doc.RootElement;
                string hValue = "";

                if (root.TryGetProperty("h", out JsonElement hProperty))
                {
                     hValue = hProperty.GetString();
                }

                return Ok(new
                {
                    file.FileName,
                    file.ContentType,
                    hValue = hValue
                });
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }



    }
}
