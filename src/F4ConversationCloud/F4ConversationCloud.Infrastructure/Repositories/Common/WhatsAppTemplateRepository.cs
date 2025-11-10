using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
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
                var dp = new DynamicParameters();

                string headerType = "", headerFormat = "", headerText = "", headerExample = "", headerMediaUrl = "";
                string bodyType = "", bodyText = "", bodyExample = "";
                string footerType = "", footerText = "";

                if (request.components != null)
                {
                    foreach (var component in request.components)
                    {
                        string? type = component?.Type?.ToString()?.ToLower();
                        if (type == null) continue;

                        switch (type)
                        {
                            case "header":
                                headerType = component.Type ?? "";
                                headerFormat = component.Format ?? "";

                                if (headerFormat.Equals("text", StringComparison.OrdinalIgnoreCase))
                                {
                                    headerText = component.Text ?? "";
                                    headerExample = string.Join(",", component.Example?.Header_Text ?? new List<string>());
                                }
                                else if (headerFormat.Equals("image", StringComparison.OrdinalIgnoreCase))
                                {
                                    headerMediaUrl = string.Join(",", component.Example?.HeaderFile ?? new List<string>());
                                }
                                break;

                            case "body":
                                bodyType = component.Type ?? "";
                                bodyText = component.Text ?? "";
                                bodyExample = component.Body_Example?.Body_Text != null
                                    ? string.Join(" | ", component.Body_Example.Body_Text)
                                    : "";
                                break;

                            case "footer":
                                footerType = component.Type ?? "";
                                footerText = component.Text ?? "";
                                break;
                        }
                    }
                }

                var parameters = new (string, object)[]
                {
                    ("@HeaderType", headerType),
                    ("@HeaderFormat", headerFormat),
                    ("@HeaderText", headerText),
                    ("@HeaderExample", headerExample),
                    ("@HeaderMediaUrl", headerMediaUrl),
                    ("@HeaderMediaId", ""),
                    ("@HeaderFileName", ""),
                    ("@HeaderLatitude", ""),
                    ("@HeaderLongitude", ""),
                    ("@HeaderAddress", ""),
                    ("@BodyType", bodyType),
                    ("@BodyText", bodyText),
                    ("@BodyExample", bodyExample),
                    ("@FooterType", footerType),
                    ("@FooterText", footerText),
                    ("@CreatedBy", "System"),
                    ("@WABAID", request.WABAID),
                    ("@ClientInfoId", request.ClientInfoId)
                };

                foreach (var (key, value) in parameters)
                    dp.Add(key, value ?? string.Empty);

                dp.Add("@TemplateName", request.name ?? string.Empty);
                dp.Add("@LanguageCode", request.language ?? string.Empty);
                dp.Add("@Category", request.category ?? string.Empty);

                return await _repository.InsertUpdateAsync("sp_Create_Campaign", dp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[InsertTemplatesListAsync] Error: {ex.Message}");
                return 0;
            }
        }
    }
}
