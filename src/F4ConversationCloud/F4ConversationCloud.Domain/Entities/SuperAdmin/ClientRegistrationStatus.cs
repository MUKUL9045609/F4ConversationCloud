using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Entities.SuperAdmin
{
    public enum ClientRegistrationStatus
    {
        [Description("Pre-Registration")]
        [Display(Name = "Pre-Registration")]
        PreRegistration = 1,

        [Description("Registration Completed")]
        [Display(Name = "Registration Completed")]
        RegistrationCompleted = 2
    }
}
