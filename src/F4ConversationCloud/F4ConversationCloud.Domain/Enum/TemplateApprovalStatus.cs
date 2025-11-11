using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Enum
{
    public enum TemplateApprovalStatus
    {
        [Description("Pending")]
        [Display(Name = "PENDING")]
        Pending = 1,

        [Description("Approved")]
        [Display(Name = "APPROVED")]
        Approved = 2,

        [Description("Rejected")]
        [Display(Name = "REJECTED")]
        Rejected = 3,

        [Description("Paused")]
        [Display(Name = "PAUSED")]
        Paused = 4,

        [Description("Failed")]
        [Display(Name = "FAILED")]
        Failed = 5,

        [Description("In Review")]
        [Display(Name = "IN REVIEW")]
        InReview = 6

    }
}
