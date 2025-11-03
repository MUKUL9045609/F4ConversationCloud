using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.Client
{
    public interface IAddPhoneNumberService
    {
        Task<IEnumerable<AddPhoneNumberModel>> GetWhatsAppProfilesByUserId();
        Task<Tuple<IEnumerable<AddPhoneNumberModel>, int>> GetFilteredWhatsAppProfilesByUserId(AddPhoneNumberListFilter filter);
    }
}
