using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Enum
{
    public enum TemplateAccess
    {
        [Display(Name = "IsCreateTemplate")]
        CreateTemplate = 1,

        [Display(Name = "IsDeleteTemplate")]
        DeleteTemplate = 2,

        [Display(Name = "IsEditTemplate")]
        EditTemplate = 3,

        [Display(Name = "IsView")]
        ViewTemplate = 4,
    }
}
