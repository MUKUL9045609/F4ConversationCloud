﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class ClientManagementListItemModel
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public string ClientName { get; set; }
        public string Status { get; set; }
        public string ApprovalStatus { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }
        public DateTime OnboardingOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
