using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates
{
    public class TemplateBaseResponse
    {
        [JsonPropertyName("data")]
        public List<TemplateData> Data { get; set; }
        [JsonPropertyName("components")]
        public List<CreateTemplateComponent> Components { get; set; }

        [JsonPropertyName("paging")]
        public TemplatePaging Paging { get; set; }
    }
    public class TemplateData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("components")]
        public List<CreateTemplateComponent> Components { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Status { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public string Id { get; set; }

        [JsonPropertyName("cards")]
        public List<Cards> cards { get; set; }
    }
    public class TemplatePaging
    {
        [JsonPropertyName("cursors")]
        public TemplateCursors Cursors { get; set; }
    }
    public class TemplateCursors
    {
        [JsonPropertyName("before")]
        public string Before { get; set; }

        [JsonPropertyName("after")]
        public string After { get; set; }
    }
}
