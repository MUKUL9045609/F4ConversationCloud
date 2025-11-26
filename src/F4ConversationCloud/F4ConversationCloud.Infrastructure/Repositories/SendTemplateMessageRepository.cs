using Dapper;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Repositories
{
    public class SendTemplateMessageRepository
    {
        private readonly IGenericRepository<string> _repository;
        public SendTemplateMessageRepository(IGenericRepository<string> repository)
        {
            _repository = repository;
        }


        public async Task<int> Insert(WhatsAppAccountTableModel request)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ClientInfoId", request.BusinessId);
                parameters.Add("WABAId", request.WABAId);
                parameters.Add("BusinessCategory", request.BusinessCategory);
                parameters.Add("WhatsAppDisplayName", request.WhatsAppDisplayName);
                parameters.Add("PhoneNumberId", request.PhoneNumberId);
                parameters.Add("PhoneNumber", request.PhoneNumber);
                parameters.Add("Status", request.Status);

                return await _repository.GetCountAsync("Sp_InsertTemplateInsights", parameters);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
