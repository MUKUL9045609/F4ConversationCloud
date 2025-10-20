using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public class APILoginResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public APILoginData Data { get; set; } = new APILoginData();
    }
    public class APILoginData
    {
        public string Token { get; set; } = string.Empty;
        public int ExpiryInSeconds { get; set; }
    }
}
