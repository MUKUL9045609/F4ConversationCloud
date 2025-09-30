using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class ClientUserManagementListFilter
    {
        public string BusinessFilter { get; set; } = string.Empty;
        public string NameFilter { get; set; } = string.Empty;
        public string EmailFilter { get; set; } = string.Empty;
        public int RoleFilter { get; set; } = 0;
        public string CreatedOnFilter { get; set; } = string.Empty;
        public string UpdatedOnFilter { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
