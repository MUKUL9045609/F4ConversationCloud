using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.WebUI.Handler;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.Templates
{
    public class MessageTemplateDTO
    {
        public string? name { get; set; }
        public string? language { get; set; }
        public string category { get; set; }

        public List<dynamic> components { get; set; } = new List<dynamic>();

    }

    public class HeadersComponent
    {
        [JsonPropertyName("Type")]
        public string? type { get; set; } = null;

        [JsonPropertyName("Format")]
        public string? format { get; set; } = null;

        [JsonPropertyName("Text")]
        public string? text { get; set; } = null;

        [JsonPropertyName("Example")]
        public HeadersExample? example { get; set; } = null;
    }

    public class HeadersImageComponent
    {
        [JsonPropertyName("Type")]
        public string? type { get; set; } = null;

        [JsonPropertyName("Format")]
        public string? format { get; set; } = null;

        [JsonPropertyName("Example")]
        public HeadersImageExample? example { get; set; } = null;
    }

    public class BodysComponent
    {
        [JsonPropertyName("Type")]
        public string? type { get; set; } = null;

        [JsonPropertyName("Text")]
        public string? text { get; set; } = null;

        [JsonPropertyName("Body_Example")]
        public BodysExample? example { get; set; } = null;
    }

    public class FootersComponent
    {
        [JsonPropertyName("Type")]
        public string? type { get; set; } = null;

        [JsonPropertyName("Text")]
        public string? text { get; set; } = null;
    }

    public class ButtonsComponent
    {
        [JsonPropertyName("Type")]
        public string? type { get; set; } = null;

        [JsonPropertyName("Buttons")]
        public List<Button>? buttons { get; set; } = null;
    }

    public class HeadersExample
    {
        [JsonPropertyName("Header_Text")]
        public List<string>? header_text { get; set; } = null;

        
    }


    public class HeadersImageExample
    {
        [JsonPropertyName("HeaderFile")]
        public List<string>? header_handle { get; set; } = null;


    }
    public class BodysExample
    {
        [JsonPropertyName("Body_Text")]
        public List<List<string>>? body_text { get; set; } = null;
    }

    public class Buttons
    {
        [JsonPropertyName("Type")]
        public string? type { get; set; } = null;

        [JsonPropertyName("Text")]
        public string? text { get; set; } = null;
    }

}
