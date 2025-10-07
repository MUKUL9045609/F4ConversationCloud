using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class MasterTaxService : IMasterTaxService
    {
        private readonly IMasterTaxRepository _masterTaxRepository;
        public MasterTaxService(IMasterTaxRepository masterTaxRepository)
        {
            _masterTaxRepository = masterTaxRepository;
        }

        public async Task<int> CreateUpdateAsync(MasterTax masterTax)
        {
            int id = await _masterTaxRepository.CreateUpdateAsync(new MasterTax()
            {
                Id = masterTax.Id,
                SGST = masterTax.SGST,
                CGST = masterTax.CGST,
                IGST = masterTax.IGST
            }); 

            return id;
        }

        public async Task<MasterTax> GetMasterTaxAsync()
        {
            MasterTax master = await _masterTaxRepository.GetMasterTaxAsync();

            if (master is null) return null;

            return new MasterTax
            {
                Id = master.Id,
                SGST = master.SGST,
                CGST = master.CGST,
                IGST = master.IGST
            };
        }
    }
}
