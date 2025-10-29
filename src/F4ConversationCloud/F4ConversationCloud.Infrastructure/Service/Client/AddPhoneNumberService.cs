using F4ConversationCloud.Application.Common.Interfaces.Repositories.Client;
using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using F4ConversationCloud.Application.Common.Models.ClientModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service.Client
{
    public class AddPhoneNumberService : IAddPhoneNumberService
    {
        private readonly IAddPhoneNumberRepository _repository;
        public AddPhoneNumberService(IAddPhoneNumberRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AddPhoneNumberModel>> GetWhatsAppProfilesByUserId()
        {
            return await _repository.GetWhatsAppProfilesByUserId();
        }
    }
}
