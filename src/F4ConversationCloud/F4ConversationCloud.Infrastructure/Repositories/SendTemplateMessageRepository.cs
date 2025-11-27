using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Infrastructure.Interfaces;

namespace F4ConversationCloud.Infrastructure.Repositories
{
    public class SendTemplateMessageRepository : ISendTemplateMessageRepository
    {
        private readonly IGenericRepository<string> _repository;
        public SendTemplateMessageRepository(IGenericRepository<string> repository)
        {
            _repository = repository;
        }

        public async Task<int> InsertIntoTemplateInsights(string PhoneNumberId , int TemplateId , int ConversationType ,string MessageSentFrom ,string MessageSentTo , string MessageSentStatus)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@PhoneNumberId", PhoneNumberId);
                parameters.Add("@TemplateId", TemplateId);
                parameters.Add("@ConversationType", ConversationType);
                parameters.Add("@MessageSentFrom", MessageSentFrom);
                parameters.Add("@MessageSentTo", MessageSentTo);
                parameters.Add("@MessageSentStatus", MessageSentStatus);

                return await _repository.GetCountAsync("sp_InsertTemplateInsights", parameters);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
