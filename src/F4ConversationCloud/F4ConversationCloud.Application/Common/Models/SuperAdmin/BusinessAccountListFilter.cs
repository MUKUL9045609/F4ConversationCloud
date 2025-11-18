using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class BusinessAccountListFilter
    {
        public string OrganizationsFilter { get; set; } = string.Empty;
        public string OnboardingOnFilter { get; set; } = string.Empty;
        public string PhoneNumberFilter { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    
}
