using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Domain.Helpers;
using F4ConversationCloud.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using System;
using System.Dynamic;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace F4ConversationCloud.Infrastructure.Repositories
{
    public class TemplateRepositories : ITemplateRepositories
    {
        private readonly IAPILogService _logService;
        private readonly DbContext _context;
        private IConfiguration _configuration { get; }

        private ITemplateService _templateService { get; }
        private IFileUploadService _fileUploadService { get; }

        private IWhatsAppTemplateRepository _whatsAppTemplateRepository { get; }
        public TemplateRepositories(IClientManagementService clientManagement, IAPILogService logService, ITemplateService templateService,
            IWhatsAppTemplateRepository whatsAppTemplateRepository, DbContext context, IFileUploadService fileUploadService)
        {
            _logService = logService;
            _templateService = templateService;
            _whatsAppTemplateRepository = whatsAppTemplateRepository;
            _context = context;
            _fileUploadService = fileUploadService;
        }

        public async Task<dynamic> MetaCreateTemplate(TemplateRequest requestBody)
        {
            var headerFile = requestBody.TemplateHeader?.Example?.HeaderFile?.FirstOrDefault().ToString();

            try
            {
                if (requestBody != null)
                {
                    if (!string.IsNullOrEmpty(requestBody.TemplateHeader.Example.HeaderFile?.ToString()))
                    {

                        string headerFileJsonString = await _templateService.UploadMetaImage(requestBody.TemplateHeader.Example.HeaderFile.FirstOrDefault().ToString(), requestBody.TemplateHeader.Example.HeaderFileName, requestBody.TemplateHeader.Example.Format);
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
                    var FileUrl = _fileUploadService.SaveFileFromBase64Async(headerFile).Result;
                    var FileName = requestBody.TemplateHeader.Example.HeaderFileName;
                    var resId = response.result.id?.ToString();

                    messageTemplate.category = response.result.category;

                    var id = await _whatsAppTemplateRepository.InsertTemplatesListAsync(messageTemplate, resId, requestBody.ClientInfoId, requestBody.CreatedBy, requestBody.WABAID, FileUrl?.ToString(), requestBody.TemplateTypes, FileName);

                    if (requestBody.TemplateButton != null && id > 0)
                    {
                        requestBody.TemplateId = id;
                        var flag = AddMetTemplateButtons(requestBody);
                    }

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

        public async Task<bool> AddMetTemplateButtons(dynamic requestBody)
        {
            MessageTemplateButtonDTO messageTemplateButtonDTO = new MessageTemplateButtonDTO();
            try
            {
                var result = await _whatsAppTemplateRepository.DeleteTemplatesButtonAsync(requestBody.TemplateId);

                foreach (var e in requestBody.TemplateButton.Buttons)
                {
                    messageTemplateButtonDTO.TemplateId = requestBody.TemplateId;
                    messageTemplateButtonDTO.ButtonCategory = e.ButtonCategory;
                    messageTemplateButtonDTO.ButtonType = e.ButtonType;
                    messageTemplateButtonDTO.ButtonText = e.Text;
                    messageTemplateButtonDTO.ButtonUrl = e.Url;
                    messageTemplateButtonDTO.ButtonUrlExample = e.Example?[0];
                    messageTemplateButtonDTO.ButtonPhoneNumber = e.Phone_Number;
                    messageTemplateButtonDTO.ButtonActionType = e.ButtonActionType;
                    messageTemplateButtonDTO.ButtonUrlType = e.ButtonUrlType;
                    messageTemplateButtonDTO.CountryCode = e.CountryCode;
                    messageTemplateButtonDTO.ButtonActiveForDays = e.ActiveForDays;
                    messageTemplateButtonDTO.CopyCode = e.CopyCode;
                    var id = await _whatsAppTemplateRepository.InsertTemplatesButtonAsync(messageTemplateButtonDTO);
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public async Task<dynamic> MetaEditTemplate(EditTemplateRequest requestBody)
        {
            var headerFile = requestBody.TemplateHeader?.Example?.HeaderFile?.FirstOrDefault().ToString();

            try
            {
                if (requestBody != null)
                {
                    if (!string.IsNullOrEmpty(requestBody.TemplateHeader.Example.HeaderFile?.ToString()))
                    {

                        string headerFileJsonString = await _templateService.UploadMetaImage(requestBody.TemplateHeader.Example.HeaderFile.FirstOrDefault().ToString(), requestBody.TemplateHeader.Example.HeaderFileName, requestBody.TemplateHeader.Example.Format);
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

                if (response.Status)
                {
                    var FileUrl = _fileUploadService.SaveFileFromBase64Async(headerFile).Result;
                    messageTemplate.category = response.result.category;
                    var resId = response.result.id?.ToString();
                    var FileName = requestBody.TemplateHeader.Example.HeaderFileName;
                    var id = await _whatsAppTemplateRepository.UpdateTemplatesAsync(messageTemplate, resId, FileUrl, FileName);

                    if (requestBody.TemplateButton != null)
                    {
                        requestBody.TemplateId = id;
                        var flag = AddMetTemplateButtons(requestBody);
                    }

                    return new APIResponse
                    {
                        Message = response.Message?.ToString().Trim('{', '}'),
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
                templateRequest.Category = EnumExtensions.GetDisplayNameById<TemplateModuleType>(model.TemplateCategory).ToUpper();
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
                string messageBody = model.MessageBody ?? string.Empty;
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
                    templateRequest.TemplateBody.Body_Example = new F4ConversationCloud.Application.Common.Models.Templates.BodyExample
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
                if (model.File != null)
                {
                    var fileString = await CommonHelper.GenerateFileToBase64String(model.File);
                    templateRequest.TemplateHeader.Example.HeaderFile = [fileString];
                    templateRequest.TemplateHeader.Example.Format = "IMAGE";
                    templateRequest.TemplateHeader.Format = "IMAGE";
                    templateRequest.TemplateHeader.Example.HeaderFileName = model.File.FileName;
                }
                templateRequest.TemplateButton.Type = "BUTTONS";

                var buttons = new List<Application.Common.Models.Templates.Button>();

                foreach (var b in model.buttons)
                {
                    var button = new Application.Common.Models.Templates.Button();

                    button.ButtonCategory = b.ButtonCategory;
                    if (b.ButtonCategory == (int)ButtonCategory.Custom)
                    {
                        button.ButtonActionType = "QUICK_REPLY";
                    }
                    else if (b.ButtonCategory == (int)ButtonCategory.VisitWebsite)
                    {
                        button.ButtonActionType = "URL";
                        button.Url = b.WebsiteUrl;
                        button.ButtonUrlType = b.UrlType;
                    }
                    else if (b.ButtonCategory == (int)ButtonCategory.CallPhoneNumber)
                    {
                        button.ButtonActionType = "PHONE_NUMBER";
                        button.CountryCode = b.CountryCode;
                        if (button.CountryCode > 0)
                        {
                            var code = EnumExtensions.GetEnumDescription((CountryCode)b.CountryCode);
                            button.Phone_Number = code + b.PhoneNumber;
                        }
                    }
                    else if (b.ButtonCategory == (int)ButtonCategory.CallOnWhatsApp)
                    {
                        button.ButtonActionType = "CALL_ON_WHATSAPP";
                        button.ActiveForDays = b.ActiveFor;
                    }
                    else if (b.ButtonCategory == (int)ButtonCategory.CopyOfferCode)
                    {
                        button.ButtonActionType = "COPY_CODE";
                        button.CopyCode = b.OfferCode;
                    }
                    button.ButtonType = b.ButtonType;
                    button.Text = b.ButtonText;

                    buttons.Add(button);
                }
                templateRequest.TemplateButton.Buttons = buttons;

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
                if (model.File != null)
                {
                    var fileString = await CommonHelper.GenerateFileToBase64String(model.File);
                    templateRequest.TemplateHeader.Example.HeaderFile = [fileString];
                    templateRequest.TemplateHeader.Example.Format = "IMAGE";
                    templateRequest.TemplateHeader.Format = "IMAGE";
                }
                templateRequest.TemplateButton.Type = "BUTTONS";

                var buttons = new List<Application.Common.Models.Templates.Button>();

                foreach (var b in model.buttons)
                {
                    var button = new Application.Common.Models.Templates.Button();

                    button.ButtonActionType = b.ButtonCategory == (int)ButtonCategory.Custom ? "QUICK_REPLY" : "";
                    button.ButtonCategory = b.ButtonCategory;
                    button.ButtonType = b.ButtonType;
                    button.Text = b.ButtonText;

                    buttons.Add(button);
                }
                templateRequest.TemplateButton.Buttons = buttons;

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


        public async Task<bool> MetaTemplateAnalyticsSync()
        {
            try
            {
                var TemplateDetails = await _whatsAppTemplateRepository.GetTemplateListForSyncUsages();
                //TemplateDetails = TemplateDetails.Where(x => x.TemplateId == 1148137137282481).ToList();

                if (TemplateDetails == null || !TemplateDetails.Any())
                    return true;

                foreach (var Template in TemplateDetails)
                {
                    long start = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    long end = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    //long start = new DateTimeOffset(2025, 10, 6, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();
                    //long end = new DateTimeOffset(2025, 10, 6, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();

                    var SycSndTempteDts = await _templateService.SyncSendTemplateDetails(
                        start.ToString(), end.ToString(),
                        Template.TemplateId.ToString(), Template.WABAID.ToString()
                    );

                    if (SycSndTempteDts?.Data?.template_analytics?.data == null
                        || SycSndTempteDts.Data.template_analytics.data.Count == 0)
                        continue;

                    var analyticsGroup = SycSndTempteDts.Data.template_analytics.data[0];
                    if (analyticsGroup?.data_points == null)
                        continue;

                    foreach (var SndTempteDts in analyticsGroup.data_points)
                    {
                        TemplateAnalyticsDTO TADTO = new TemplateAnalyticsDTO();
                        TADTO.TemplateRead = SndTempteDts.read; 
                        TADTO.TemplateSend = SndTempteDts.sent;
                        TADTO.TemplateDelivered = SndTempteDts.delivered; 
                        TADTO.TemplateReplied = SndTempteDts.replied;
                        TADTO.TemplateAmountSpent = Convert.ToDecimal(SndTempteDts.cost?[0]?.value); 
                        TADTO.TemplateCostPerDelivered = Convert.ToDecimal(SndTempteDts.cost?[1]?.value); 
                        TADTO.TemplateCostPerUrlButtonClick = Convert.ToDecimal(SndTempteDts.cost?[2]?.value);
                        TADTO.TemplateUrlButtonClickCount = Convert.ToInt16(SndTempteDts.clicked?[0]?.count); 
                        TADTO.TemplateUniqueUrlButtonClickCount = Convert.ToInt16(SndTempteDts.clicked?[1]?.count); 
                        TADTO.TemplateSendFrom = DateTimeOffset.FromUnixTimeSeconds(SndTempteDts.start).UtcDateTime; 
                        TADTO.TemplateSendTo = DateTimeOffset.FromUnixTimeSeconds(SndTempteDts.end).UtcDateTime; 
                        TADTO.WABAID = Template.WABAID;
                        TADTO.ClientInfoId = Template.ClientInfoId; 
                        TADTO.TemplateId = Template.TemplateId;
                        var response = await _whatsAppTemplateRepository.InsertIntoTemplateAnalytics(TADTO);
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

