using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Repositories
{
    public class TemplateRepositories : ITemplateRepositories
    {
        private readonly IAPILogService _logService;
        private readonly DbContext _context;
        private IConfiguration _configuration { get; }

        private ITemplateService _templateService { get; }

        private IWhatsAppTemplateRepository _whatsAppTemplateRepository { get; }
        public TemplateRepositories(IClientManagementService clientManagement, IAPILogService logService, ITemplateService templateService,
            IWhatsAppTemplateRepository whatsAppTemplateRepository, DbContext context)
        {
            _logService = logService;
            _templateService = templateService;
            _whatsAppTemplateRepository = whatsAppTemplateRepository;
            _context = context;
        }

        public async Task<dynamic> MetaCreateTemplate(TemplateRequest requestBody)
        {
            try
            {
                if (requestBody != null)
                {

                    //if(requestBody.TemplateHeader.Example.HeaderFile != null)
                    //{
                    //    requestBody.TemplateHeader.Example.HeaderFile = await _templateService.UploadMetaImage(requestBody.TemplateHeader.Example.HeaderFile.ToString());
                    //}

                    if (!string.IsNullOrEmpty(requestBody.TemplateHeader.Example.HeaderFile?.ToString()))
                    {

                        string headerFileJsonString = await _templateService.UploadMetaImage(requestBody.TemplateHeader.Example.HeaderFile.FirstOrDefault().ToString());
                        using JsonDocument doc = JsonDocument.Parse(headerFileJsonString);
                        JsonElement root = doc.RootElement;

                        if (root.TryGetProperty("h", out JsonElement hProperty))
                        {
                            string hValue = hProperty.GetString();
                            requestBody.TemplateHeader.Example.HeaderFile.Clear();
                            requestBody.TemplateHeader.Example.HeaderFile.Add(hValue);
                        }
                    }
                }

                MessageTemplateDTO messageTemplate = _templateService.TryDeserializeAndAddComponent(requestBody);

                var response = await _templateService.CreateTemplate(messageTemplate, requestBody.WABAID);

                if (response.Status)
                {
                    messageTemplate.category = response.result.category;
                    var resId = response.result.id?.ToString();
                    var id = await _whatsAppTemplateRepository.InsertTemplatesListAsync(messageTemplate, resId, requestBody.ClientInfoId, requestBody.CreatedBy, requestBody.WABAID);

                    return new APIResponse
                    {
                        Message = "Template created successFully.",
                        Status = true
                    };
                }
                else
                {

                    return new APIResponse
                    {
                        Message = response.Message?.ToString().Trim('{', '}'),
                        Status = false
                    };
                }


            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Message = "Error occured while creating template",
                    Status = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }

        public async Task<dynamic> MetaEditTemplate(EditTemplateRequest requestBody)
        {
            try
            {
                if (requestBody != null)
                {
                    if (!string.IsNullOrEmpty(requestBody.TemplateHeader.Example.HeaderFile?.ToString()))
                    {

                        string headerFileJsonString = await _templateService.UploadMetaImage(requestBody.TemplateHeader.Example.HeaderFile.FirstOrDefault().ToString());
                        using JsonDocument doc = JsonDocument.Parse(headerFileJsonString);
                        JsonElement root = doc.RootElement;

                        if (root.TryGetProperty("h", out JsonElement hProperty))
                        {
                            string hValue = hProperty.GetString();
                            requestBody.TemplateHeader.Example.HeaderFile.Clear();
                            requestBody.TemplateHeader.Example.HeaderFile.Add(hValue);
                        }
                    }
                }

                MessageTemplateDTO messageTemplate = _templateService.TryDeserializeAndAddComponent(requestBody);


                var response = await _templateService.EditTemplate(messageTemplate, requestBody.TemplateId);

                if (response.Status == true)
                {
                    messageTemplate.category = response.result.category;
                    var resId = response.result.id?.ToString();
                    var id = await _whatsAppTemplateRepository.UpdateTemplatesAsync(messageTemplate, resId);

                    return new APIResponse
                    {
                        Message = "Template created successFully.",
                        Status = true
                    };

                }
                else
                {
                    return new APIResponse
                    {
                        Message = response.Message?.ToString().Trim('{', '}'),
                        Status = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Message = "Error occured while creating template",
                    Status = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }

        public async Task<dynamic> BuildAndCreateTemplate(TemplateViewRequestModel model)
        {
            try
            {
                var templateRequest = new TemplateRequest();

                templateRequest.Name = model.TemplateName;
                templateRequest.Language = EnumExtensions.GetDisplayNameById<TemplateLanguages>(model.Language);
                //templateRequest.Category = EnumExtensions.GetDisplayNameById<TemplateModuleType>(model.TemplateCategory);
                templateRequest.Category = "UTILITY";
                templateRequest.TemplateHeader.Type = "HEADER";
                templateRequest.TemplateHeader.Format = "TEXT";
                templateRequest.TemplateHeader.Text = model.Header;
                templateRequest.TemplateHeader.Example = new HeaderExample
                {
                    Header_Text = new List<string> { model.HeaderVariableValue },
                    Format = "TEXT"
                };
                templateRequest.TemplateBody.Type = "BODY";
                templateRequest.TemplateBody.Text = model.MessageBody;
                string messageBody = model.MessageBody ?? string.Empty; ;
                var matches = Regex.Matches(messageBody, @"\{\{(\d+)\}\}");

                if (matches.Count > 0)
                {
                    // Get distinct variable numbers found in the body
                    var variableNumbers = matches
                        .Select(m => int.Parse(m.Groups[1].Value))
                        .Distinct()
                        .OrderBy(n => n)
                        .ToList();

                    // Build a map from BodyVariableValue ("{{1}}") to BodyVariableName ("the end of August")
                    // Ensure model.bodyVariables is populated from your partial before POST.
                    var valueToName = model.bodyVariables
                        .Where(v => !string.IsNullOrWhiteSpace(v.BodyVariableValue))
                        .ToDictionary(
                            v => v.BodyVariableValue.Trim(), // key: "{{1}}"
                            v => (v.BodyVariableName ?? string.Empty).Trim() // value: sample text
                        );

                    // Determine the max variable number present
                    int maxVar = variableNumbers.Max();

                    // Prepare the ordered samples: index 0 => {{1}}, index 1 => {{2}}, ...
                    var orderedSamples = new List<string>(capacity: maxVar);
                    for (int n = 1; n <= maxVar; n++)
                    {
                        string key = $"{{{{{n}}}}}"; // "{{n}}"
                        if (valueToName.TryGetValue(key, out var sample) && !string.IsNullOrEmpty(sample))
                        {
                            orderedSamples.Add(sample);
                        }
                    }

                    // Finally set the Body_Example with one inner list aligned by {{1}}, {{2}}, ...
                    templateRequest.TemplateBody.Body_Example = new Application.Common.Models.Templates.BodyExample
                    {
                        Body_Text = new List<List<string>> { orderedSamples }
                    };
                }
                else
                {
                    // No variables in body; omit Body_Example or set null
                    templateRequest.TemplateBody.Body_Example = null;
                }
                //templateRequest.TemplateBody.Text = "Shop now through {{1}} and use code {{2}} to get {{3}} off of all merchandise.\r\n";

                templateRequest.TemplateFooter.type = "FOOTER";
                templateRequest.TemplateFooter.text = model.Footer;
                templateRequest.ClientInfoId = model.ClientInfoId.ToString();
                templateRequest.WABAID = model.WABAId;
                templateRequest.CreatedBy = _context.SessionUserId.ToString();

                APIResponse result = await MetaCreateTemplate(templateRequest);

                return result;
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Message = "Error occured while creating template",
                    Status = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }

        public async Task<dynamic> BuildAndEditTemplate(TemplateViewRequestModel model)
        {
            try
            {
                var templateRequest = new EditTemplateRequest();

                templateRequest.Name = model.TemplateName;
                templateRequest.Language = EnumExtensions.GetDisplayNameById<TemplateLanguages>(model.Language);
                //templateRequest.Category = EnumExtensions.GetDisplayNameById<TemplateModuleType>(model.TemplateCategory);
                templateRequest.Category = "UTILITY";
                templateRequest.TemplateHeader.Type = "HEADER";
                templateRequest.TemplateHeader.Format = "TEXT";
                templateRequest.TemplateHeader.Text = model.Header;
                templateRequest.TemplateHeader.Example = new HeaderExample
                {
                    Header_Text = new List<string> { model.HeaderVariableValue },
                    Format = "TEXT"
                };
                templateRequest.TemplateBody.Type = "BODY";
                templateRequest.TemplateBody.Text = model.MessageBody;
                string messageBody = model.MessageBody ?? string.Empty; ;
                var matches = Regex.Matches(messageBody, @"\{\{(\d+)\}\}");

                if (matches.Count > 0)
                {
                    // Get distinct variable numbers found in the body
                    var variableNumbers = matches
                        .Select(m => int.Parse(m.Groups[1].Value))
                        .Distinct()
                        .OrderBy(n => n)
                        .ToList();

                    // Build a map from BodyVariableValue ("{{1}}") to BodyVariableName ("the end of August")
                    // Ensure model.bodyVariables is populated from your partial before POST.
                    var valueToName = model.bodyVariables
                        .Where(v => !string.IsNullOrWhiteSpace(v.BodyVariableValue))
                        .ToDictionary(
                            v => v.BodyVariableValue.Trim(), // key: "{{1}}"
                            v => (v.BodyVariableName ?? string.Empty).Trim() // value: sample text
                        );

                    // Determine the max variable number present
                    int maxVar = variableNumbers.Max();

                    // Prepare the ordered samples: index 0 => {{1}}, index 1 => {{2}}, ...
                    var orderedSamples = new List<string>(capacity: maxVar);
                    for (int n = 1; n <= maxVar; n++)
                    {
                        string key = $"{{{{{n}}}}}"; // "{{n}}"
                        if (valueToName.TryGetValue(key, out var sample) && !string.IsNullOrEmpty(sample))
                        {
                            orderedSamples.Add(sample);
                        }
                    }

                    // Finally set the Body_Example with one inner list aligned by {{1}}, {{2}}, ...
                    templateRequest.TemplateBody.Body_Example = new Application.Common.Models.Templates.BodyExample
                    {
                        Body_Text = new List<List<string>> { orderedSamples }
                    };
                }
                else
                {
                    // No variables in body; omit Body_Example or set null
                    templateRequest.TemplateBody.Body_Example = null;
                }
                //templateRequest.TemplateBody.Text = "Shop now through {{1}} and use code {{2}} to get {{3}} off of all merchandise.\r\n";

                templateRequest.TemplateFooter.type = "FOOTER";
                templateRequest.TemplateFooter.text = model.Footer;
                templateRequest.ClientInfoId = model.ClientInfoId.ToString();
                templateRequest.WABAID = model.WABAId;
                templateRequest.CreatedBy = _context.SessionUserId.ToString();
                templateRequest.TemplateId = model.TemplateId;

                APIResponse result = await MetaEditTemplate(templateRequest);

                return result;
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Message = "Error occured while creating template",
                    Status = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }

        public async Task<bool> MetaSyncTemplate()
        {
            try
            {
                var metaDetails = await _whatsAppTemplateRepository.GetMetaUsersConfiguration();

                foreach (var meta in metaDetails)
                {
                    var wabaId = meta.WabaId;
                    var TemplateDetails = await _templateService.GetAllTemplatesAsync(wabaId);
                    foreach (var Details in TemplateDetails)
                    {
                        var response = await _whatsAppTemplateRepository.SyncAndUpdateWhatsappTemplate(Details.TemplateId, Details.Category, Details.Status);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }
    }
}

