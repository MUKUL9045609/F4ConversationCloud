using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface ITemplateService
    {
        Task<dynamic> CreateTemplate(MessageTemplateDTO requestBody, string WABAID);
        Task<dynamic> EditTemplate(MessageTemplateDTO requestBody, string TemplateId);
        Task<dynamic> DeleteTemplate(int TemplateId, string TemplateName);
        Task<dynamic> DeleteTemplateByName(string TemplateName);
        MessageTemplateDTO TryDeserializeAndAddComponent(dynamic request);
        Task<dynamic> UploadMetaImage(string base64Image);
        Task<dynamic> Whatsappbusinessprofile(string profilepicturehandle, string PhoneNumberId);
        Task<dynamic> GetWhatsappbusinessprofile(string PhoneNumberId);
        Task<dynamic> SyncTemplateByTemplateID(string TemplateId);
        Task<dynamic> GetAllTemplatesAsync(string wabaId);
    }
}
