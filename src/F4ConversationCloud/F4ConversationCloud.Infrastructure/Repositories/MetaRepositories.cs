using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Repositories
{
    public class MetaRepositories : IMetaRepositories
    {

        private readonly DbContext _context;
        private readonly IGenericRepository<AddPhoneNumberModel> _repository;
        public MetaRepositories(DbContext context, IGenericRepository<AddPhoneNumberModel> repository)
        {
            _context = context;
            _repository = repository;
        }


        public async Task<int> UpdateMetaUsersConfigurationDetails(WhatsAppAccountTableModel request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("BusinessId", request.BusinessId);
            parameters.Add("WABAId", request.WABAId);
            parameters.Add("BusinessCategory", request.BusinessCategory);
            parameters.Add("WhatsAppDisplayName", request.WhatsAppDisplayName);
            parameters.Add("PhoneNumberId", request.PhoneNumberId);
            parameters.Add("PhoneNumber", request.PhoneNumber);
            parameters.Add("Status", request.Status);
         
            return await _repository.GetCountAsync("sp_InsertOrUpdate_WhatsAppAccount", parameters);
        }
    }
}
