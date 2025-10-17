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
        public string WebHookUrl { get; set; }
        public int TemplateType { get; set; }
        public bool Create { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool All { get; set; }
        public bool AllowUserManagement { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumer { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string ClientTimeZone { get; set; }
        public string Stage { get; set; }
        public string Role { get; set; }

    }
}
