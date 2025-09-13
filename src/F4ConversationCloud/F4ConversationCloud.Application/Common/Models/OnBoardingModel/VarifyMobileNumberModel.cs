using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel
{
    public class VarifyMobileNumberModel
    {
        public string UserEmailId { get; set; }
        public string UserPhoneNumber { get; set; }
        public string OTP { get; set; }
        public string OTP_Source { get; set; }
    }

    public class VarifyUserDetailsResponse
    {
        public bool status { get; set; }
        public string message { get; set; }

        public SendSmsResponse smsResponse { get; set; }
        public EmailSendResponse emailResponse { get; set; }

    }
    public class SendSmsResponse
    {
        public string Sid { get; set; }
        public string Status { get; set; }
        public string To { get; set; }
        public string From { get; set; }
    }
    public class EmailSendResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}
