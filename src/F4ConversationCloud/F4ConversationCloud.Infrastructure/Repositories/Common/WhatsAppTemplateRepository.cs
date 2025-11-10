using BuldanaUrban.Domain.Helpers;
using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Converters;

namespace F4ConversationCloud.Infrastructure.Repositories.Common
{
    public class WhatsAppTemplateRepository: IWhatsAppTemplateRepository
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
                Dp.Add("@category", filter.Category != 0 ? filter.Category.Get<DisplayAttribute>().Name : string.Empty);
                Dp.Add("@langCode", filter.LangCode);
                Dp.Add("@modifiedOn", filter.ModifiedOn);
                Dp.Add("@Templatestatus", filter.TemplateStatus != 0 ? filter.TemplateStatus.Get<DisplayAttribute>().Name: string.Empty);
                Dp.Add("@PageNumber", filter.PageNumber);
                Dp.Add("@PageSize", filter.PageSize);
                var serialize = Serializers.JsonObject(filter);
                 var templateList = await _repository.GetListByValuesAsync<WhatsappTemplateListItem>("[sp_GetWhatsappTemplateList]", Dp);
                int TotalCount = await _repository.GetByValuesAsync<int>("sp_GetWhatsappTemplateCount", Dp);
                return (templateList, TotalCount);

            }
            catch (Exception)
            {

                return (Enumerable.Empty<WhatsappTemplateListItem>(),0);
            }
           
        }

        public async Task<WhatsappTemplateDetail> GetTemplateByIdAsync(string Template_id)
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
