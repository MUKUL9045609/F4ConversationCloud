using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Enum
{
    public enum CustomButtonType
    {
        [Description("Custom")]
        [Display(Name = "Custom")]
        Custom = 1,

        [Description("Pre-configured response")]
        [Display(Name = "Pre-configured response")]
        PreconfiguredResponse = 2
    }
}
