using Dapper;
using F4ConversationCloud.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Repositories
{
    public class SendTemplateMessageRepository
    {
        public SendTemplateMessageRepository()
        {

        }


        //public async Task<int> Insert(WhatsAppAccountTableModel request)
        //{
        //    DynamicParameters parameters = new DynamicParameters();

        //    parameters.Add("BusinessId", request.BusinessId);
        //    parameters.Add("WABAId", request.WABAId);
        //    parameters.Add("BusinessCategory", request.BusinessCategory);
        //    parameters.Add("WhatsAppDisplayName", request.WhatsAppDisplayName);
        //    parameters.Add("PhoneNumberId", request.PhoneNumberId);
        //    parameters.Add("PhoneNumber", request.PhoneNumber);
        //    parameters.Add("Status", request.Status);

        //    return await _repository.GetCountAsync("sp_InsertOrUpdate_WhatsAppAccount", parameters);
        //}
    }
}
