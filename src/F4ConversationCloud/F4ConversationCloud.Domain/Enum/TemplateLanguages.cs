using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Enum
{
    public enum TemplateLanguages
    {
        [Description("English")]
        [Display(Name = "en")]
        English = 1,

        [Description("Hindi")]
        [Display(Name = "Hindi")]
        Hindi = 2,

        [Description("Marathi")]
        [Display(Name = "Marathi")]
        Marathi = 3
    }
}
