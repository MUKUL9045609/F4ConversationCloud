using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates
{
    public class WhatsAppTemplateRequest
    {
        //[JsonPropertyName("data")]

        //public TemplateData Data { get; set; }

        [JsonPropertyName("components")]
        public List<CreateTemplateComponent> Components { get; set; }

        [JsonPropertyName("cards")]
        public List<Cards> cards { get; set; }

        //[JsonPropertyName("paging")]
        //public TemplatePaging Paging { get; set; }
 

        [JsonPropertyName("name")]
        public string Name { get; set; }



        [JsonPropertyName("language")]
        public string Language { get; set; }



        [JsonPropertyName("category")]
        public string Category { get; set; }

        public int ClientInfoId { get; set; }
    }


    public class CreateTemplateComponent
    {
       
        [JsonPropertyName("example")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Example? Example { get; set; }

        [JsonPropertyName("format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Format { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }


        [JsonPropertyName("buttons")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<TemplateButton> Buttons { get; set; }

        [JsonPropertyName("cards")]
        public List<Cards> cards { get; set; }
    }
    public class TemplateButton
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
    }
    public class Example
    {
        [JsonPropertyName("body_text")]
        public List<List<string>> BodyText { get; set; }



        [JsonPropertyName("header_text")]
        public List<string> HeaderText { get; set; } = new();

        [JsonPropertyName("header_handle")]
        public string[] HeaderHandle { get; set; }
        //[JsonPropertyName("body_text_named_params")]
        //public List<BodyTextNamedParam> BodyTextNamedParams { get; set; }


    }
    public class BodyTextNamedParam
    {
        [JsonPropertyName("param_name")]
        public string ParamName { get; set; }

        [JsonPropertyName("example")]
        public string Example { get; set; }
    }

    public class Cards {

        [JsonPropertyName("components")]
        public List<CreateTemplateComponent> Components { get; set; }

    }
}
