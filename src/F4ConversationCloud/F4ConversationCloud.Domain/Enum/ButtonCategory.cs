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
        VisitWebsite = 2,

        [Description("Call on WhatsApp")]
        [Display(Name = "Call on WhatsApp")]
        CallOnWhatsApp = 3,

        [Description("Call phone number")]
        [Display(Name = "Call phone number")]
        CallPhoneNumber = 4,

        [Description("Complete Flow")]
        [Display(Name = "Complete Flow")]
        CompleteFlow = 5,

        [Description("Copy offer code")]
        [Display(Name = "Copy offer code")]
        CopyOfferCode = 6
    }
}
