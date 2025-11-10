using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.CommonModels
{
    public class WhatsappTemplateDetail
    {
        

            public string TemplateName { get; set; }

            public string LanguageCode { get; set; }

            public string Category { get; set; }

            public string HeaderType { get; set; }

            public string HeaderFormat { get; set; }

            public string HeaderText { get; set; }

            public string HeaderExample { get; set; }

            public string HeaderMediaUrl { get; set; }

            public string HeaderMediaId { get; set; }

            public string HeaderFileName { get; set; }

            public string HeaderLatitude { get; set; }

            public string HeaderLongitude { get; set; }

            public string HeaderAddress { get; set; }

            public string BodyType { get; set; }

            public string BodyText { get; set; }

            public string BodyExample { get; set; }

            public string FooterType { get; set; }

            public string FooterText { get; set; }

            public string CreatedBy { get; set; }

            public DateTime CreatedOn { get; set; }

            public DateTime? ModifiedOn { get; set; }

            public bool IsActive { get; set; }
       
    }
}
