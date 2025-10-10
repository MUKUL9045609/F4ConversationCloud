using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Enum
{
    public enum VariableTypes
    {
        [Description("Name")]
        [Display(Name = "Name")]
        Name = 1,

        [Description("Number")]
        [Display(Name = "Number")]
        Number = 2
    }
}
