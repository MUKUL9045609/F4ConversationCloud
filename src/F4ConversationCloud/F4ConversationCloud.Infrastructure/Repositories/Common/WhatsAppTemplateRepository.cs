using BuldanaUrban.Domain.Helpers;
using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Infrastructure.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Converters;

namespace F4ConversationCloud.Infrastructure.Repositories.Common
{
    public class WhatsAppTemplateRepository : IWhatsAppTemplateRepository
    {
        private readonly IGenericRepository<string> _repository;
        private readonly IWhatsAppTemplateParser _parser;
        public WhatsAppTemplateRepository(IGenericRepository<string> repository , IWhatsAppTemplateParser whatsAppTemplateParser)
        {
            _repository = repository;
            _parser = whatsAppTemplateParser;
        }

        public async Task<(IEnumerable<WhatsappTemplateListItem> Templates, int TotalCount)> GetTemplatesListAsync(WhatsappTemplateListFilter filter)
        {
            try
            {
                DynamicParameters Dp = new DynamicParameters();

                Dp.Add("@ClientInfoId", filter.ClientInfoId);
                Dp.Add("@templateName", filter.TemplateName);
                Dp.Add("@category", filter.Category);
                Dp.Add("@langCode", filter.LangCode);
                Dp.Add("@modifiedOn", filter.ModifiedOn);
                Dp.Add("@Templatestatus", filter.TemplateStatus);
                Dp.Add("@PageNumber", filter.PageNumber);
                Dp.Add("@PageSize", filter.PageSize);

                var serialize = Serializers.JsonObject(filter);

                var templateList = await _repository.GetListByValuesAsync<WhatsappTemplateListItem>("[sp_GetWhatsappTemplateList]", Dp);
                int TotalCount = await _repository.GetByValuesAsync<int>("sp_GetWhatsappTemplateCount", Dp);

                return (templateList, TotalCount);
            }
            catch (Exception)
            {
                return (Enumerable.Empty<WhatsappTemplateListItem>(), 0);
            }
        }


        public async Task<WhatsappTemplateDetail> GetTemplateByIdAsync(int Template_id)
        {
            try
            {
                DynamicParameters Dp = new DynamicParameters();
                Dp.Add("@TemplateId", Template_id);
                
                var templateById = await _repository.GetByValuesAsync<WhatsappTemplateDetail>("sp_GetWhatsappTemplate", Dp);
                
                return templateById;
            }
            catch (Exception)
            {
                return new WhatsappTemplateDetail();
            }

        }

        public async Task<int> InsertTemplatesListAsync(MessageTemplateDTO request)
        {
            try
            {
                var data = _parser.Parse(request);

                var parameters = new DynamicParameters();
                parameters.Add("@TemplateName", request.name ?? "", DbType.String);
                parameters.Add("@Category", Enum.TryParse<TemplateModuleType>(request.category, true, out var cat) ? (int)cat : (int)TemplateModuleType.Utility, DbType.Int32);
                parameters.Add("@LanguageCode", Enum.TryParse<TemplateLanguages>(request.language, true, out var lang) ? (int)lang : (int)TemplateLanguages.English, DbType.Int32);

                parameters.Add("@HeaderType", data.HeaderType, DbType.String);
                parameters.Add("@HeaderFormat", data.HeaderFormat, DbType.String);
                parameters.Add("@HeaderText", data.HeaderText, DbType.String);
                parameters.Add("@HeaderExample", data.HeaderExample, DbType.String);
                parameters.Add("@HeaderMediaUrl", data.HeaderMediaUrl, DbType.String);
                parameters.Add("@HeaderMediaId", null, DbType.String);
                parameters.Add("@HeaderFileName", null, DbType.String);
                parameters.Add("@HeaderLatitude", null, DbType.Decimal);
                parameters.Add("@HeaderLongitude", null, DbType.Decimal);
                parameters.Add("@HeaderAddress", null, DbType.String);

                parameters.Add("@BodyType", data.BodyType, DbType.String);
                parameters.Add("@BodyText", data.BodyText, DbType.String);
                parameters.Add("@BodyExample", data.BodyExample, DbType.String);

                parameters.Add("@FooterType", data.FooterType, DbType.String);
                parameters.Add("@FooterText", data.FooterText, DbType.String);

                parameters.Add("@CreatedBy", request.CreatedBy, DbType.String);
                parameters.Add("@WABAID", request.WABAID, DbType.String);
                parameters.Add("@ClientInfoId", request.ClientInfoId, DbType.String);
                parameters.Add("@Templateid", request.TemplateId, DbType.String);
                parameters.Add("@TemplateStatus", Enum.TryParse<TemplateApprovalStatus>(request.language, true, out var TemplateApprovalStatus) ? (int)TemplateApprovalStatus : (int)TemplateLanguages.English, DbType.Int32);


                return await _repository.InsertUpdateAsync("sp_InsertWhatsappTemplate", parameters);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetCountAsync(TemplateListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("WABAId", filter.WABAId);
            parameters.Add("TemplateNameFilter", filter.TemplateNameFilter);
            parameters.Add("TemplateCategoryFilter", filter.TemplateCategoryFilter);
            parameters.Add("LanguageFilter", filter.LanguageFilter);
            parameters.Add("CreatedOnFilter", filter.CreatedOnFilter);
            parameters.Add("StatusFilter", filter.StatusFilter);

            return await _repository.GetCountAsync("sp_GetTemplateCountByWABAId", parameters);
        }

        public async Task<IEnumerable<TemplateModel>> GetFilteredAsync(TemplateListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("WABAId", filter.WABAId);
            parameters.Add("TemplateNameFilter", filter.TemplateNameFilter);
            parameters.Add("TemplateCategoryFilter", filter.TemplateCategoryFilter);
            parameters.Add("LanguageFilter", filter.LanguageFilter);
            parameters.Add("CreatedOnFilter", filter.CreatedOnFilter);
            parameters.Add("StatusFilter", filter.StatusFilter);
            parameters.Add("pageNumber", filter.PageNumber);
            parameters.Add("pageSize", filter.PageSize);

            return await _repository.GetListByValuesAsync<TemplateModel>("sp_GetFilteredTemplatesByWABAId", parameters);
        }
    }
}
