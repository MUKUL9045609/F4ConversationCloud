using F4ConversationCloud.Application.Common.Meta.BussinessProfile;
using F4ConversationCloud.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.Meta
{
    public interface IMetaService
    {
        Task<PhoneRegistrationOnMetaResponse> RegisterPhone(PhoneRegistrationOnMeta request);
        Task<PhoneRegistrationOnMetaResponse> DeregisterPhone(PhoneRegistrationOnMeta request);
        Task<dynamic> GetBusinessUsersWithWhatsappAccounts();
    }
}
