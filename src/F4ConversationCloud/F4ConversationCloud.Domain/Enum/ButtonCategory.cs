using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.Domain.Enum
{
    public enum ButtonCategory
    {
        [Description("Custom")]
        [Display(Name = "Custom")]
        Custom = 1,

        [Description("Visit website")]
        [Display(Name = "Visit website")]
        VisitWebsite = 2
    }
}
