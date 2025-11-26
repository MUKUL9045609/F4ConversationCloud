using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Enum
{
    public enum VisitwebsiteUrlType
    {
        [Description("Static")]
        [Display(Name = "Static")]
        Static = 1,

        //[Description("Dynamic")]
        //[Display(Name = "Dynamic")]
        //Dynamic = 2
    }
}
