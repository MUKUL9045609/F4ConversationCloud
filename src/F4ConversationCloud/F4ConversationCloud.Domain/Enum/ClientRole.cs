using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Enum
{
    public enum ClientRole
    {
        [Description("Admin")]
        [Display(Name = "Admin")]
        Admin = 1
    }
}
