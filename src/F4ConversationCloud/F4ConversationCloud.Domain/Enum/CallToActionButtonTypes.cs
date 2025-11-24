using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Enum
{
    public enum CallToActionButtonTypes
    {
        [Description("Visit website")]
        [Display(Name = "Visit website")]
        VisitWebsite = 1,

        [Description("Call on WhatsApp")]
        [Display(Name = "Call on WhatsApp")]
        CallOnWhatsApp = 2,

        [Description("Call phone number")]
        [Display(Name = "Call phone number")]
        CallPhoneNumber = 3,

        [Description("Complete Flow")]
        [Display(Name = "Complete Flow")]
        CompleteFlow = 4,

        [Description("Copy offer code")]
        [Display(Name = "Copy offer code")]
        CopyOfferCode = 5
    }
}
