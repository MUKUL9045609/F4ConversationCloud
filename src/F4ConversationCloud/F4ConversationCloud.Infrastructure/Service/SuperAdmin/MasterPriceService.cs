using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Infrastructure.Repositories.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class MasterPriceService : IMasterPriceService
    {
        private readonly IMasterPriceRepository _masterPriceRepository;
        public MasterPriceService(IMasterPriceRepository masterPriceRepository)
        {
            _masterPriceRepository = masterPriceRepository;
        }

        public async Task<int> CreateAsync(MasterPrice masterPrice)
        {
            int id = await _masterPriceRepository.CreateAsync(new MasterPrice()
            {
                Id = masterPrice.Id,
                ConversationType = masterPrice.ConversationType,
                Price = masterPrice.Price,
                FromDate = masterPrice.FromDate,
                ToDate = masterPrice.ToDate
            });

            return id;
        }

        public async Task<Tuple<IEnumerable<MasterPrice>, int>> GetFilteredMasterPrices(MasterListFilter filter)
        {
            return Tuple.Create(await _masterPriceRepository.GetFilteredAsync(filter), await _masterPriceRepository.GetCountAsync(filter));
        }
    }
}
