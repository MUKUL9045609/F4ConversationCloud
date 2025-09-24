using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class ClientDetails
    {
        public int Id { get; set; }
        public string PhoneNumberId { get; set; }
        public string WABAId { get; set; }
        public string BusinessId { get; set; }
        public int ClientInfoId { get; set; }
        public string BusinessName { get; set; }
        public string Status { get; set; }
        public string PhoneNumber { get; set; }
        public string AppVersion { get; set; }
        public string ApprovalStatus { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string RegisteredFirstName { get; set; }
        public string RegisteredLastName { get; set; }
        public string RegisteredEmail { get; set; }
        public string RegisteredPhoneNumber { get; set; }
        public string RegisteredAddress { get; set; }
        public string RegisteredCountry { get; set; }
        public string RegisteredTimeZone{ get; set; }
        public string WebHookUrl{ get; set; }
        public bool IsMarketing { get; set; }
        public bool IsAuthentication { get; set; }
        public bool IsUtility { get; set; }
        public bool MarketingCreate { get; set; }
        public bool MarketingAdd { get; set; }
        public bool MarketingEdit { get; set; }
        public bool MarketingDelete { get; set; }
        public bool MarketingAll { get; set; }
        public bool AuthenticationCreate { get; set; }
        public bool AuthenticationAdd { get; set; }
        public bool AuthenticationEdit { get; set; }
        public bool AuthenticationDelete { get; set; }
        public bool AuthenticationAll { get; set; }
        public bool UtilityCreate { get; set; }
        public bool UtilityAdd { get; set; }
        public bool UtilityEdit { get; set; }
        public bool UtilityDelete { get; set; }
        public bool UtilityAll { get; set; }
    }
}
