using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Repositories.SuperAdmin
{
    public class MasterTaxRepository : IMasterTaxRepository
    {
        private readonly IGenericRepository<MasterTax> _genericRepository;
        private readonly DbContext _dbContext;
        public MasterTaxRepository(IGenericRepository<MasterTax> genericRepository, DbContext dbContext) {
            _genericRepository = genericRepository;
            _dbContext = dbContext;
        }

        public async Task<int> CreateUpdateAsync(MasterTax masterTax)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("userId", _dbContext.SessionUserId);
            parameters.Add("id", masterTax.Id);
            parameters.Add("sGST", masterTax.SGST);
            parameters.Add("cGST", masterTax.CGST);
            parameters.Add("iGST", masterTax.IGST);

            return await _genericRepository.InsertUpdateAsync("sp_CreateUpdateMasterTax", parameters);
        }

        public async Task<MasterTax> GetMasterTaxAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await _genericRepository.GetByIdAsync("sp_GetMasterTax", parameters);
        }
    }
}
