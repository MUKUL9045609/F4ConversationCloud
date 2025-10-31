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
    Draft = 1,

    [Display(Name = "Client Registered")]
    ClientRegistered = 2,

    [Display(Name = "Meta Registered")]
    MetaRegistered = 3,

    
}
