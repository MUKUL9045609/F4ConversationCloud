using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string OTP { get; set; }
    }

    public class SmsRequest
    {
        public string ToPhoneNumber { get; set; }
       // public string OTP { get; set; }
        public string Text { get; set; }
    }
}
    
