using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.Templates;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Repositories
{
    public class TemplateRepositories : ITemplateRepositories
    {
        private readonly IAPILogService _logService;
        private IConfiguration _configuration { get; }

        private ITemplateService _templateService { get; }

        private IWhatsAppTemplateRepository _whatsAppTemplateRepository { get; }
        public TemplateRepositories(IClientManagementService clientManagement, IAPILogService logService , ITemplateService templateService , IWhatsAppTemplateRepository whatsAppTemplateRepository)
        {
            _logService = logService;
            _templateService = templateService;
            _whatsAppTemplateRepository = whatsAppTemplateRepository;
        }

        public async Task<dynamic> MetaCreateTemplate(TemplateRequest requestBody)
        {
            try
            {
                if (requestBody != null) { 
                
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
                
                var response = await _templateService.CreateTemplate(messageTemplate ,  requestBody.WABAID);

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
    }

}

