using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;

namespace F4ConversationCloud.Infrastructure.Repositories.SuperAdmin
{
    public class MasterPriceRepository : IMasterPriceRepository
    {
        private readonly IGenericRepository<MasterPrice> _repository;
        private readonly DbContext _context;
        public MasterPriceRepository(IGenericRepository<MasterPrice> repository, DbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<int> CreateAsync(MasterPrice masterPrice)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("userId", _context.SessionUserId);
            parameters.Add("id", masterPrice.Id);
            parameters.Add("conversationType", masterPrice.ConversationType);
            parameters.Add("price", masterPrice.Price);
            parameters.Add("fromDate", masterPrice.FromDate);
            parameters.Add("toDate", masterPrice.ToDate);

            return await _repository.InsertUpdateAsync("sp_CreateMasterPrice", parameters);
        }

        public async Task<IEnumerable<MasterPrice>> GetFilteredAsync(MasterListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("pageNumber", filter.PageNumber);
            parameters.Add("pageSize", filter.PageSize);

            return await _repository.GetListByValuesAsync<MasterPrice>("sp_GetFilteredMasterPrices", parameters);
        }

        public async Task<int> GetCountAsync(MasterListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            return await _repository.GetCountAsync("sp_GetMasterPriceCount", parameters);
        }

        public async Task<List<MasterPrice>> GetLatestRecordsByConversationType()
        {
            DynamicParameters parameters = new DynamicParameters();
            return (await _repository.GetListByParamAsync<MasterPrice>("GetLatestRecordsByConversationType", parameters)).ToList();
        }
    }
}
