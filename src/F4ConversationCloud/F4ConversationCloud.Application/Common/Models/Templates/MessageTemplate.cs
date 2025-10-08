using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.Templates
{
    public class MessageTemplate
    {
        public string? name { get; set; }
        public string? language { get; set; }
        public string? category { get; set; }
        public List<dynamic> components { get; set; }

    }

    public class HeaderComponent
    {
        public string? type { get; set; } = null;
        public string? format { get; set; } = null;
        public string? text { get; set; } = null;
        public HeaderExample? example { get; set; } = null;
    }

    public class BodyComponent
    {
        public string? type { get; set; } = null;
        public string? text { get; set; } = null;
        public BodyExample? example { get; set; } = null;
    }

    public class FooterComponent
    {
        public string? type { get; set; } = null;
        public string? text { get; set; } = null;        
    }

    public class ButtonComponent
    {
        public string? type { get; set; } = null;
        public List<Button>? buttons { get; set; } = null;
    }

    public class HeaderExample
    {
        public List<string>? header_text { get; set; } = null;
    }

    public class BodyExample
    {
        public List<List<string>>? body_text { get; set; } = null;
    }

    public class Button
    {
        public string? type { get; set; } = null;
        public string? text { get; set; } = null;
    }

}

