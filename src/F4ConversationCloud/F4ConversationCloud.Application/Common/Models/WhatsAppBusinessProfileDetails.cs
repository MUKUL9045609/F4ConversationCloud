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
        [JsonProperty("about")]
        public string About { get; set; }

        [JsonProperty("business_category")]
        public string BusinessCategory { get; set; }
    }

    //public class PhoneNumber
    //{
    //    [JsonProperty("id")]
    //    public string Id { get; set; }

    //    [JsonProperty("display_phone_number")]
    //    public string DisplayPhoneNumber { get; set; }

    //    [JsonProperty("verified_name")]
    //    public string VerifiedName { get; set; }

    //    [JsonProperty("name_status")]
    //    public string NameStatus { get; set; }

    //    [JsonProperty("code_verification_status")]
    //    public string CodeVerificationStatus { get; set; }

    //    [JsonProperty("whatsapp_business_profile")]
    //    public WhatsAppBusinessProfileDetails WhatsappBusinessProfile { get; set; }
    //}

    public class AssignedWABA
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phone_numbers")]
        public List<PhoneNumber> PhoneNumbers { get; set; }
    }

    //public class BusinessUser
    //{
    //    [JsonProperty("id")]
    //    public string Id { get; set; }

    //    [JsonProperty("name")]
    //    public string Name { get; set; }

    //    [JsonProperty("assigned_whatsapp_business_accounts")]
    //    public List<AssignedWABA> AssignedWhatsAppBusinessAccounts { get; set; }
    //}

    public class MetaResponse
    {
        [JsonProperty("business_users")]
        public List<BusinessUser> BusinessUsers { get; set; }
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

    public class Root
    {
        public BusinessUsers business_users { get; set; }
        public string id { get; set; }
    }

    public class BusinessUsers
    {
        public List<BusinessUser> data { get; set; }
        public Paging paging { get; set; }
    }

    public class BusinessUser
    {
        public string id { get; set; }
        public string name { get; set; }
        public AssignedWaba assigned_whatsapp_business_accounts { get; set; }
    }

    public class AssignedWaba
    {
        public List<Waba> data { get; set; }
        public Paging paging { get; set; }
    }

    public class Waba
    {
        public string name { get; set; }
        public string id { get; set; }
        public PhoneNumbers phone_numbers { get; set; }
    }

    public class PhoneNumbers
    {
        public List<PhoneNumber> data { get; set; }
        public Paging paging { get; set; }
    }

    public class PhoneNumber
    {
        public string display_phone_number { get; set; }
        public string verified_name { get; set; }
        public string name_status { get; set; }
        public string code_verification_status { get; set; }
        public string id { get; set; }
    }

    public class Paging
    {
        public Cursors cursors { get; set; }
    }

    public class Cursors
    {
        public string before { get; set; }
        public string after { get; set; }
    }
}
