using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Client;
using F4ConversationCloud.Application.Common.Models.ClientModel;
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

        public async Task<int> GetCountAsync(AddPhoneNumberListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("userId", _context.SessionUserId);
            parameters.Add("WABAIdFilter", filter.WABAIdFilter);
            parameters.Add("BusinessIdFilter", filter.BusinessIdFilter);
            parameters.Add("BusinessCategoryFilter", filter.BusinessCategoryFilter);
            parameters.Add("WhatsappDisplayNameFilter", filter.WhatsappDisplayNameFilter);
            parameters.Add("PhoneNumberIdFilter", filter.PhoneNumberIdFilter);
            parameters.Add("PhoneNumberFilter", filter.PhoneNumberFilter);
            parameters.Add("statusFilter", filter.StatusFilter);

            return await _repository.GetCountAsync("sp_GetWhatsAppProfilesCountByUserId", parameters);
        }

        public async Task<IEnumerable<AddPhoneNumberModel>> GetFilteredAsync(AddPhoneNumberListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("userId", _context.SessionUserId);
            parameters.Add("WABAIdFilter", filter.WABAIdFilter);
            parameters.Add("BusinessIdFilter", filter.BusinessIdFilter);
            parameters.Add("BusinessCategoryFilter", filter.BusinessCategoryFilter);
            parameters.Add("WhatsappDisplayNameFilter", filter.WhatsappDisplayNameFilter);
            parameters.Add("PhoneNumberIdFilter", filter.PhoneNumberIdFilter);
            parameters.Add("PhoneNumberFilter", filter.PhoneNumberFilter);
            parameters.Add("statusFilter", filter.StatusFilter);
            parameters.Add("pageNumber", filter.PageNumber);
            parameters.Add("pageSize", filter.PageSize);

            return await _repository.GetListByValuesAsync<AddPhoneNumberModel>("sp_GetFilteredWhatsAppProfilesByUserId", parameters);
        }
    }
}
