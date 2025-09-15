using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel
{
    public class ValidateRegistrationOTPModel
    {
        public string? UserEmailId { get; set; }
        public string? UserPhoneNumber { get; set; }
        public string OTP { get; set; }
    }
    public class ValidateRegistrationOTPResponse
    {
        public bool status { get; set; }
    }
}
