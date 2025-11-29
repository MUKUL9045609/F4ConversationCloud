using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.Common
{
    public interface IWhatsAppTemplateRepository
    {
        Task<(IEnumerable<WhatsappTemplateListItem> Templates, int TotalCount)> GetTemplatesListAsync(WhatsappTemplateListFilter filter);
        Task<WhatsappTemplateDetail> GetTemplateByIdAsync(int Template_id);
        Task<IEnumerable<TemplatesButtonsListItem>> WhatsappTemplatesButtons(int Template_id);
        Task<IEnumerable<TemplateModel>> GetFilteredAsync(TemplateListFilter filter);
        Task<int> GetCountAsync(TemplateListFilter filter);
        //Task<int> InsertTemplatesListAsync(MessageTemplateDTO request, string TemplateId, string ClientInfoId, string CreatedBy, string WABAID);
        Task<int> InsertTemplatesListAsync(MessageTemplateDTO request, string TemplateId, string ClientInfoId, string CreatedBy, string WABAID, string HeaderFileUrl, string TemplateTypes, string HeaderFileFilename);
        Task<int> UpdateTemplatesAsync(MessageTemplateDTO request, string TemplateId, string HeaderFileUrl , string HeaderFileFilename);
        Task<dynamic> GetMetaUsersConfiguration();
        Task<int> SyncAndUpdateWhatsappTemplate(string TemplateId, string Templatecategory, string TemplateStatus);
        Task<int> DeactivateTemplateAsync(int templateId);
        Task<int> ActivateTemplateAsync(int templateId);
        Task<int> InsertTemplatesButtonAsync(MessageTemplateButtonDTO request);
        Task<IEnumerable<TemplateModel.Button>> GetTemplateButtonsAsync(int MetaConfigId, int TemplateId);
        Task<bool> DeleteTemplatesButtonAsync(int WhatsappTemplateId);
        Task<IEnumerable<TemplateListForSync>> GetTemplateListForSyncUsages();

        Task<int> InsertIntoTemplateAnalytics(TemplateAnalyticsDTO templateAnalyticsDTO);
    }
}
