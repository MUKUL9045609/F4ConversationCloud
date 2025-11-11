using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public class APIResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
        public APIData Data { get; set; } = new APIData();
    }
    public class APIData
    {
        public string Token { get; set; } = string.Empty;
        public int ExpiryInSeconds { get; set; }
    }
}
