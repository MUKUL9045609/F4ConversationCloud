using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel
{
    public class ValidateRegistrationOTPModel
    {
        public string? UserEmailId { get; set; }
        public string? UserPhoneNumber { get; set; }

        [Required(ErrorMessage = "OTP is required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must be a 6-digit number")]
        public string OTP { get; set; }
        public string CountryCode { get; set; }
    }
    public class ValidateRegistrationOTPResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}
