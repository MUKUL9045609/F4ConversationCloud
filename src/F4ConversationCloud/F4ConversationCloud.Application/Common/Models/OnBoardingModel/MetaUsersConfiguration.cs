using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel
{
    public class MetaUsersConfiguration
    {
        public int OnboardingUserId { get; set; }
        public string CompanyName { get; set; }
        public string WabaId { get; set; }
        public string PhoneNumberId { get; set; }
        public string BusinessId { get; set; }
        public string AppName { get; set; }
        public string AppVersion { get; set; } = "v23.0";
        public string AccessToken { get; set; }
        public DateTime? TokenExpiresAt { get; set; }
    }
    public class MetaUsersConfigurationResponse
    {
        public bool status { get; set; }
        public string Message { get; set; }
    }
}
