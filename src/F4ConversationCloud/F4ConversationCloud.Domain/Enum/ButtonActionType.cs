using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace F4ConversationCloud.Domain.Enum
{
    public enum ButtonActionType
    {
        [Description("Quick Reply")]
        [Display(Name = "QUICK_REPLY")]
        QUICK_REPLY = 1,

        [Description("Visit website")]
        [Display(Name = "URL")]
        URL = 2,
            
        [Description("Phone Number")]
        [Display(Name = "PHONE_NUMBER")]
        PHONE_NUMBER = 3
    }
}
