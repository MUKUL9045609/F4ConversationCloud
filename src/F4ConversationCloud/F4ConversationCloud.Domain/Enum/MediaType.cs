using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Enum
{
    public enum MediaType
    {
        [Description("None")]
        [Display(Name = "None")]
        None = 0,

        [Description("Image")]
        [Display(Name = "Image")]
        Image = 1,

        [Description("Video")]
        [Display(Name = "Video")]
        Video = 2,

        [Description("Document")]
        [Display(Name = "Document")]
        Document = 3,

        [Description("Location")]
        [Display(Name = "Location")]
        Location = 4
    }
}
