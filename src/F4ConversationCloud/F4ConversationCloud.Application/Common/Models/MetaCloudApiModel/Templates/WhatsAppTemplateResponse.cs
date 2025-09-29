using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates
{
    public class TemplateResponse : TemplateBaseResponse
    {
        public Dictionary<string, object> AdditionalFields { get; set; }
    }
    

    public class WhatsAppTemplateResponse
    {
        [JsonPropertyName("data")]
        public TemplateData Data { get; set; }

        [JsonPropertyName("components")]
        public List<WhatsAppBusinessHSMWhatsAppHSMComponentGet> Components { get; set; }

        [JsonPropertyName("paging")]
        public TemplatePaging Paging { get; set; }
    }
    
}
