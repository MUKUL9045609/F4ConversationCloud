using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class ClientsMetaConfigurationsListItemModel
    {
        public int Id { get; set; }
        public  string WhatsAppBotName { get; set; }
        public string Status { get; set; }
        public string PhoneNumberId { get; set; }
        public string WabaId { get; set; }
        public string BusinessId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
