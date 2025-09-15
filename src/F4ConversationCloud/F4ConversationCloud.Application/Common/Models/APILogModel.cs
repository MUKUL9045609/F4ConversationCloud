using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public class APILogModel
    {
        public string Name { get; set; } = string.Empty;
        public string APIUrl { get; set; } = string.Empty;
        public string MethodType { get; set; } = string.Empty;
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public string Query { get; set; } = string.Empty;
        public string Request { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
    }
}
