using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.FlexApi.V1;

namespace F4ConversationCloud.Infrastructure.Service.Common
{
    public class WhatsAppTemplateService : IWhatsAppTemplateService
    {
        private readonly IWhatsAppTemplateRepository _templateRepository;
        private readonly ILogService _logService;
        public WhatsAppTemplateService(IWhatsAppTemplateRepository templateRepository, ILogService logService)
        {
            _templateRepository = templateRepository;
            _logService = logService;
        }

        public async Task<WhatsAppTemplateResponse> GetTemplatesListAsync(WhatsappTemplateListFilter filter)
        {

            try
            {
                var (templates, totalCount) = await _templateRepository.GetTemplatesListAsync(filter);

                return new WhatsAppTemplateResponse
                {
                    Templates = templates,
                    TotalCount = totalCount
                };
            }
            catch (Exception ex)
            {
                var log = new ClientAdminLogsModel
                {
                    Source = "WhatsappTemplate/GetTemplatesListAsync",
                    Data = $"Filter: {JsonConvert.SerializeObject(filter)}",
                    LogType = "Error",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };
                await _logService.InsertClientAdminLogsAsync(log);
                return new WhatsAppTemplateResponse
                {
                    Templates = Enumerable.Empty<WhatsappTemplateListItem>(),
                    TotalCount = 0
                };
            }

        }
        public async Task<WhatsappTemplateDetail> GetTemplateByIdAsync(int templateId)
        {
            try
            {
                var templateDetail = await _templateRepository.GetTemplateByIdAsync(templateId);
                var templateButtons = await _templateRepository.WhatsappTemplatesButtons(templateId);

                templateDetail.RawHeader = templateDetail.HeaderText;
                templateDetail.RawBody = templateDetail.BodyText;
                if (templateDetail != null)
                {
                    if (!string.IsNullOrEmpty(templateDetail.BodyExample))
                    {
                        templateDetail.BodyText = await ReplaceTemplatePlaceholdersAsync(
                            templateDetail.BodyText,
                            templateDetail.BodyExample
                        );
                    }

                    if (!string.IsNullOrEmpty(templateDetail.HeaderExample))
                    {
                        templateDetail.HeaderText = await ReplaceTemplatePlaceholdersAsync(
                            templateDetail.HeaderText,
                            templateDetail.HeaderExample
                        );
                    }
                }
                templateDetail.TemplateButtons = templateButtons;
                return templateDetail;
            }
            catch (Exception ex)
            {
                var log = new ClientAdminLogsModel
                {
                    Source = "WhatsappTemplate/GetTemplateByIdAsync",
                    Data = $"TemplateId: {templateId}",
                    LogType = "Error",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };
                await _logService.InsertClientAdminLogsAsync(log);
                return null;
            }
        }

        public async Task<DeleteTemplateResponse> DeactivateTemplateAsync(int templateId)
        {
            try
            {
                var tempRes = await _templateRepository.DeactivateTemplateAsync(templateId);

                if (tempRes == 0)
                {
                    return new DeleteTemplateResponse
                    {
                        success = false,
                        message = "Failed to deactivate template."
                    };
                }

                return new DeleteTemplateResponse
                {
                    success = true,
                    message = "Template deactivated successfully."
                };
            }
            catch (Exception ex)
            {
                var log = new ClientAdminLogsModel
                {
                    Source = "WhatsappTemplate/DeactivateTemplateAsync",
                    Data = $"TemplateId: {templateId}",
                    LogType = "Error",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };

                await _logService.InsertClientAdminLogsAsync(log);

               
                return new DeleteTemplateResponse
                {
                    success = false,
                    message = "Technical Error."
                };
            }
        }

        public async Task<DeleteTemplateResponse> ActivateTemplateAsync(int templateId)
        {
            try
            {
                var tempRes = await _templateRepository.ActivateTemplateAsync(templateId);

                if (tempRes == 0)
                {
                    return new DeleteTemplateResponse
                    {
                        success = false,
                        message = "Failed to activate template."
                    };
                }

                return new DeleteTemplateResponse
                {
                    success = true,
                    message = "Template activated successfully."
                };
            }
            catch (Exception ex)
            {
                var log = new ClientAdminLogsModel
                {
                    Source = "WhatsappTemplate/ActivateTemplateAsync",
                    Data = $"TemplateId: {templateId}",
                    LogType = "Error",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };

                await _logService.InsertClientAdminLogsAsync(log);

                return new DeleteTemplateResponse
                {
                    success = false,
                    message = "Technical Error."
                };
            }
        }

        private async Task<string> ReplaceTemplatePlaceholdersAsync(string templateText, string parameters)
        {
            try
            {
                if (string.IsNullOrEmpty(templateText) || string.IsNullOrEmpty(parameters))
                    return templateText;

                var paramArray = parameters.Split(',').Select(p => p.Trim()).ToArray();

                for (int i = 0; i < paramArray.Length; i++)
                {
                    templateText = templateText.Replace($"{{{{{i + 1}}}}}", paramArray[i]);
                }

                return templateText;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<Tuple<IEnumerable<TemplateModel>, int>> GetFilteredTemplatesByWABAId(TemplateListFilter filter)
        {
            Tuple<IEnumerable<TemplateModel>, int> response = Tuple.Create(Enumerable.Empty<TemplateModel>(), 0);
            try
            {
                response = Tuple.Create(await _templateRepository.GetFilteredAsync(filter), await _templateRepository.GetCountAsync(filter));
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "WhatsappTemplate/GetFilteredAsync";
                logModel.AdditionalInfo = $"Model: {JsonConvert.SerializeObject(filter)}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
            }
            finally
            {

            }
            return response;
        }
    }
}
