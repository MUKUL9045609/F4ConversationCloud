using F4ConversationCloud.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class ActivateDeactivateClientAccountRequest
    {
        public int ClientId { get; set; }
        public int DeactivatedBy { get; set; }

        public ClientRegistrationStatus AccountStatus { get; set; }

        public string PhoneNumberID { get; set; }
        public string WhatsAppAccountStatus { get; set; } 
    }
}
