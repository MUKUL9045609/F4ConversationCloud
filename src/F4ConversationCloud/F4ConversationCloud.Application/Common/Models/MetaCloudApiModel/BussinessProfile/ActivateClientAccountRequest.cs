using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Meta.BussinessProfile
{
    public class ActivateClientAccountRequest
    {
        public string PhoneNumberId { get; set; }

        public string  Pin { get; set; }
        public string messaging_product { get; set; }
    }
    public class ActivateClientAccountResponse
    {
        public string  status { get; set; }
    }
}
