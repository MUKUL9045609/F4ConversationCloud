using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace F4ConversationCloud.Domain.Enum;
public enum ClientFormStage
{
    [Display(Name = "Draft")]
    draft = 1,

    [Display(Name = "Meta-registered")]
    metaregistered = 2,

    [Display(Name = "Admin-Approval")]
     adminapproval = 3
}
