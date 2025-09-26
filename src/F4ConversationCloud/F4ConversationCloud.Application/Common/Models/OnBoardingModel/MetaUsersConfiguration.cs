using F4ConversationCloud.Application.Common.Meta.BussinessProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel
{
    public class MetaUsersConfiguration
    {
       
        public string AppVersion { get; set; } = "v23.0";
        public string WabaId { get; set; }
        public string BusinessId { get; set; }
        public string PhoneNumberId { get; set; }
        public int ClientInfoId { get; set; }
        public string CompanyName { get; set; }
        public string WhatsAppBotName { get; set; }
        public string Status { get; set; }
        public string PhoneNumber { get; set; }

        public string ApprovalStatus { get; set; }

        public string WebSite { get; set; }
        public string ClientEmail { get; set; }
        public string Category { get; set; }

        [JsonPropertyName("data")]
        public List<BusinessProfileData> Data { get; set; }
        public WhatsappBusinessProfile whatsappBusinessProfile { get; set; }
    }
    public class WhatsappBusinessProfile
    {
        public string WebSite { get; set; }
        public string ClientEmail { get; set; }
        public string Category { get; set; }
    }

    public class MetaUsersConfigurationResponse
    {
        public bool status { get; set; }
        public string Message { get; set; }
    }
}
