using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Client;
using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;

namespace F4ConversationCloud.Infrastructure.Repositories.Client
{
    public class AddPhoneNumberRepository : IAddPhoneNumberRepository
    {
        private readonly DbContext _context;
        private readonly IGenericRepository<AddPhoneNumberModel> _repository;
        public AddPhoneNumberRepository(DbContext context, IGenericRepository<AddPhoneNumberModel> repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<IEnumerable<AddPhoneNumberModel>> GetWhatsAppProfilesByUserId()
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("userId", _context.SessionUserId);

            return await _repository.GetListByParamAsync<AddPhoneNumberModel>("sp_GetWhatsAppProfilesByUserId", parameters);
        }
    }
}
