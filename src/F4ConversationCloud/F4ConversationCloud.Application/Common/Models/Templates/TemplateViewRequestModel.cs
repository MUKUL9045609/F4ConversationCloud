using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace F4ConversationCloud.Application.Common.Models.Templates
{
    public class TemplateViewRequestModel
    {
        public string TemplateName { get; set; }
        public int TemplateCategory { get; set; }
        public string TemplateCategoryName { get; set; }
        public int TemplateType { get; set; }
        public string TemplateTypeName { get; set; }
        public int Language { get; set; }
        public int VariableType { get; set; }
        public int MediaType { get; set; }
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string Header { get; set; }
        public string MessageBody { get; set; }
        public string Footer { get; set; }
        public string HeaderVariableName { get; set; }
        public string HeaderVariableValue { get; set; }
        public List<BodyVariable> bodyVariables { get; set; } = new List<BodyVariable>();
        public int ClientInfoId { get; set; }
        public int MetaConfigId { get; set; }
        public string WABAId { get; set; }
        public string PageMode { get; set; }
        public string TemplateId { get; set; }
        public int TemplateTableId { get; set; }
        public List<Button> buttons { get; set; } = new List<Button>();
        public int ButtonCategory { get; set; }

        public class BodyVariable()
        {
            public string BodyVariableName { get; set; }
            public string BodyVariableValue { get; set; }
        }
        public class Button()
        {
            public int ButtonType { get; set; }
            public string ButtonText { get; set; }
        }
    }
}
