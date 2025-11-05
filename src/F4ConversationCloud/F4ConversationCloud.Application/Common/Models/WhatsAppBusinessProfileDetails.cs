using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public class WhatsAppBusinessProfileDetails
    {
        public string About { get; set; }
        public string Business_Category { get; set; }
    }

    public class PhoneNumber
    {
        public string Id { get; set; }

        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }
        public string Verified_Name { get; set; }
        public string Name_Status { get; set; }
        public string Code_Verification_Status { get; set; }
        public WhatsAppBusinessProfileDetails Whatsapp_Business_Profile { get; set; }
    }

    public class AssignedWABA
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<PhoneNumber> Phone_Numbers { get; set; }
    }

    public class BusinessUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<AssignedWABA> Assigned_WhatsApp_Business_Accounts { get; set; }
    }

    public class MetaResponse
    {
        public List<BusinessUser> Business_Users { get; set; }
    }

    public class WhatsAppAccountTableModel
    {
        public string BusinessId { get; set; }
        public string WABAId { get; set; }
        public string BusinessCategory { get; set; }
        public string WhatsAppDisplayName { get; set; }
        public string PhoneNumberId { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
    }
}
