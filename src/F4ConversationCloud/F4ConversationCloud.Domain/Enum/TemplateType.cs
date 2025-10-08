using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Enum
{
    public enum MarketingTemplateType
    {
        [Description("Default")]
        [Display(Name = "Default")]
        Default = 1,

        [Description("Catalogue")]
        [Display(Name = "Catalogue")]
        Catalogue = 2,

        [Description("Calling permissions request")]
        [Display(Name = "Calling permissions request")]
        CallingPermissionsRequest = 3
    }

    public enum UtilityTemplateType
    {
        [Description("Default")]
        [Display(Name = "Default")]
        Default = 1,

        [Description("Calling permissions request")]
        [Display(Name = "Calling permissions request")]
        CallingPermissionsRequest = 2
    }

    public enum AuthenticationTemplateType
    {
        [Description("One Time Passcode")]
        [Display(Name = "One Time Passcode")]
        OneTimePasscode = 1
    }
}
