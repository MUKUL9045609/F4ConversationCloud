﻿using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
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

        public TemplateRepositories(IClientManagementService clientManagement, IAPILogService logService , ITemplateService templateService )
        {
            _logService = logService;
            _templateService = templateService;
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
                var result = await _templateService.CreateTemplate(messageTemplate);

                return new
                {
                    Message = "Template created successFully.",
                    Success = true
                };

            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Template not created successFully.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
    }

}

