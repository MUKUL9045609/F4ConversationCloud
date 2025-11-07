using F4ConversationCloud.Application.Common.Models.ClientModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.Client
{
    public interface IAddPhoneNumberRepository
    {
        Task<IEnumerable<AddPhoneNumberModel>> GetWhatsAppProfilesByUserId();
        Task<IEnumerable<AddPhoneNumberModel>> GetFilteredAsync(AddPhoneNumberListFilter filter);
        Task<int> GetCountAsync(AddPhoneNumberListFilter filter);
    }
}
