using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Enum
{
    public enum TemplateModuleType
    {
        [Description("Marketing")]
        [Display(Name = "Marketing")]
        Marketing = 1,

        [Description("Authentication")]
        [Display(Name = "Authentication")]
        Authentication = 2,

        [Description("Utility")]
        [Display(Name = "Utility")]
        Utility = 3
    }
}
