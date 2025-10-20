using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Enum
{
    public enum TemplateAPIEndPoints
    {
        [Description("CreateTemplate")]
        [Display(Name = "CreateTemplate")]
        CreateTemplate = 1,

        [Description("EditTemplate")]
        [Display(Name = "EditTemplate")]
        EditTemplate = 2,

        [Description("DeleteTemplate")]
        [Display(Name = "DeleteTemplate")]
        DeleteTemplate = 3,

        [Description("DeleteTemplateByName")]
        [Display(Name = "DeleteTemplateByName")]
        DeleteTemplateByName = 4,

        [Description("TemplateList")]
        [Display(Name = "TemplateList")]
        TemplateList = 5,

        [Description("ViewTemplate")]
        [Display(Name = "ViewTemplate")]
        ViewTemplate = 6,

    }
}
