using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class BusinessAccountListItem
    {
        public int SrNo { get; set; }
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public int RegistrationStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string OrganizationsName { get; set; }
        public string ClientId { get; set; }
        public int IsActive { get; set; }
        public string Stage { get; set; }

        public int ClientWaBaAccCount { get; set; }
    }
}
