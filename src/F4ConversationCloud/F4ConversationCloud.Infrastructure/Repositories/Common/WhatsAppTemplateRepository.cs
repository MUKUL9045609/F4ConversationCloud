using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Infrastructure.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Repositories.Common
{
    public class WhatsAppTemplateRepository : IWhatsAppTemplateRepository
    {
        private readonly IGenericRepository<string> _repository;
        public WhatsAppTemplateRepository(IGenericRepository<string> repository)
        {
            _repository = repository;
        }

        public async Task<(IEnumerable<WhatsappTemplateListItem> Templates, int TotalCount)> GetTemplatesListAsync(WhatsappTemplateListFilter filter)
        {
            try
            {
                DynamicParameters Dp = new DynamicParameters();

                Dp.Add("@ClientInfoId", filter.ClientInfoId);
                Dp.Add("@templateName", filter.TemplateName);
                Dp.Add("@category", filter.Category);
                Dp.Add("@ClientInfoId", filter.ClientInfoId);
                Dp.Add("@langCode", filter.LangCode);
                Dp.Add("@modifiedOn", filter.ModifiedOn);
                Dp.Add("@Templatestatus", filter.TemplateStatus);
                Dp.Add("@PageNumber", filter.PageNumber);
                Dp.Add("@PageSize", filter.PageSize);

                var templateList = await _repository.GetListByValuesAsync<WhatsappTemplateListItem>("[sp_GetWhatsappTemplateList]", Dp);
                int TotalCount = await _repository.GetByValuesAsync<int>("sp_GetWhatsappTemplateCount", Dp);
                return (templateList, TotalCount);

            }
            catch (Exception)
            {

                return (Enumerable.Empty<WhatsappTemplateListItem>(), 0);
            }

        }

        public async Task<int> InsertTemplatesListAsync(MessageTemplateDTO request)
        {
            try
            {

                string headerType = "", headerFormat = "", headerText = "", headerExample = "", headerMediaUrl = "";
                string bodyType = "", bodyText = "", bodyExample = "";
                string footerType = "", footerText = "";

                if (request.components != null)
                {
                    foreach (var component in request.components)
                    {
                        string? type = component?.@type?.ToString()?.ToLower();
                        if (type == null) continue;

                        switch (type)
                        {
                            case "header":
                                headerType = component.@type ?? "";
                                headerFormat = component.format ?? "";

                                if (headerFormat.Equals("text", StringComparison.OrdinalIgnoreCase))
                                {
                                    headerText = component.text ?? "";
                                    headerExample = string.Join(",", component.example?.header_text ?? new List<string>());
                                }
                                else if (headerFormat.Equals("image", StringComparison.OrdinalIgnoreCase))
                                {
                                    headerMediaUrl = string.Join(",", component.example?.HeaderFile ?? new List<string>());
                                }
                                break;

                            case "body":
                                bodyType = component.@type ?? "";
                                bodyText = component.text ?? "";
                                if (component.example?.body_text is IEnumerable<IEnumerable<string>> bodyExamples)
                                {
                                    bodyExample = string.Join(" | ", bodyExamples.Select(row => string.Join(",", row)));
                                }
                                else
                                {
                                    bodyExample = "";
                                }
                                break;

                            case "footer":
                                footerType = component.@type ?? "";
                                footerText = component.text ?? "";
                                break;
                        }
                    }
                }

                var parameters = new DynamicParameters();

                parameters.Add("@TemplateName", request.name ?? string.Empty, DbType.String);

                if (Enum.TryParse<TemplateModuleType>(request.category, true, out var TemplateCategory))
                {
                    parameters.Add("@Category", (int)TemplateCategory, DbType.Int32);
                }
                else
                {
                    parameters.Add("@Category", (int)TemplateModuleType.Utility, DbType.Int32);

                }

                if (Enum.TryParse<TemplateLanguages>(request.language, true, out var Templatelanguage))
                {
                    parameters.Add("@LanguageCode", (int)Templatelanguage, DbType.Int32);
                }
                else
                {
                    parameters.Add("@LanguageCode", (int)TemplateLanguages.English, DbType.Int32);

                }

                parameters.Add("@HeaderType", headerType, DbType.String);
                parameters.Add("@HeaderFormat", headerFormat, DbType.String);
                parameters.Add("@HeaderText", headerText, DbType.String);
                parameters.Add("@HeaderExample", headerExample, DbType.String);
                parameters.Add("@HeaderMediaUrl", headerMediaUrl, DbType.String);
                parameters.Add("@HeaderMediaId", null, DbType.String);
                parameters.Add("@HeaderFileName", null, DbType.String);
                parameters.Add("@HeaderLatitude", null, DbType.Decimal);
                parameters.Add("@HeaderLongitude", null, DbType.Decimal);
                parameters.Add("@HeaderAddress", null, DbType.String);
                parameters.Add("@BodyType", bodyType, DbType.String);
                parameters.Add("@BodyText", bodyText, DbType.String);
                parameters.Add("@BodyExample", bodyExample, DbType.String);
                parameters.Add("@FooterType", footerType, DbType.String);
                parameters.Add("@FooterText", footerText, DbType.String);
                parameters.Add("@CreatedBy", request.CreatedBy, DbType.String);
                parameters.Add("@WABAID", request.WABAID, DbType.String);
                parameters.Add("@ClientInfoId", request.ClientInfoId, DbType.String);

                return await _repository.InsertUpdateAsync("sp_InsertWhatsappTemplate", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[InsertTemplatesListAsync] Error: {ex.Message}");
                return 0;
            }
        }
    }
}
