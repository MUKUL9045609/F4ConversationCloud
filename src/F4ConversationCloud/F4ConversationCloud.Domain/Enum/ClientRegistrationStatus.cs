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
    public enum ClientRegistrationStatus
    {
        [Description("Pending")]
        [Display(Name = "Pending")]
        Pending = 1,

        [Description("Active")]
        [Display(Name = "Active")]
        Active = 2,

        [Description("Deactivated")]
        [Display(Name = "Deactivated")]
        Deactivated = 3


    }
}
