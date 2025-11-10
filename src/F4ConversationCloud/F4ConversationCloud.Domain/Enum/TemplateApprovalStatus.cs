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
        [Display(Name = "Pending")]
        Pending = 1,

        [Description("Approved")]
        [Display(Name = "Approved")]
        Approved = 2,

        [Description("Rejected")]
        [Display(Name = "Rejected")]
        Rejected = 3,

        [Description("Paused")]
        [Display(Name = "Paused")]
        Paused = 4,

        [Description("Failed")]
        [Display(Name = "Failed")]
        Failed = 5,

        [Description("In Review")]
        [Display(Name = "In Review")]
        InReview = 6

    }
}
