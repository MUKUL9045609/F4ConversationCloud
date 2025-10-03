using F4ConversationCloud.Domain.Entities.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin
{
    public interface IMasterTaxService
    {
        Task<int> CreateUpdateAsync(MasterTax masterTax);
        Task<MasterTax> GetMasterTaxAsync();
    }
}
