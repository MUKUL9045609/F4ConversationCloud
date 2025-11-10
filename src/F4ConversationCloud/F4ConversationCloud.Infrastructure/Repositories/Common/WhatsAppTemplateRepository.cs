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

                var header = request.components?.FirstOrDefault(c => c?.@type?.Equals("header", StringComparison.OrdinalIgnoreCase) == true);
                var body = request.components?.FirstOrDefault(c => c?.@type?.Equals("body", StringComparison.OrdinalIgnoreCase) == true);
                var footer = request.components?.FirstOrDefault(c => c?.@type?.Equals("footer", StringComparison.OrdinalIgnoreCase) == true);

                string headerType = header?.@type ?? "";
                string headerFormat = header?.format ?? "";
                string headerText = "", headerExample = "", headerMediaUrl = "";

                if (headerFormat.Equals("text", StringComparison.OrdinalIgnoreCase))
                {
                    headerText = header?.text ?? "";
                    headerExample = string.Join(",", header?.example?.header_text ?? Array.Empty<string>());
                }
                else if (headerFormat.Equals("image", StringComparison.OrdinalIgnoreCase))
                {
                    headerMediaUrl = string.Join(",", header?.example?.HeaderFile ?? Array.Empty<string>());
                }

                string bodyType = body?.@type ?? "";
                string bodyText = body?.text ?? "";
                string bodyExample = body?.example?.body_text is IEnumerable<IEnumerable<string>> examples
                    ? string.Join(" | ", examples.Select(row => string.Join(",", row)))
                    : "";

                string footerType = footer?.@type ?? "";
                string footerText = footer?.text ?? "";

                var parameters = new DynamicParameters();
                parameters.Add("@TemplateName", request.name ?? "", DbType.String);
                parameters.Add("@Category", Enum.TryParse<TemplateModuleType>(request.category, true, out var cat) ? (int)cat : (int)TemplateModuleType.Utility, DbType.Int32);
                parameters.Add("@LanguageCode", Enum.TryParse<TemplateLanguages>(request.language, true, out var lang) ? (int)lang : (int)TemplateLanguages.English, DbType.Int32);

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
