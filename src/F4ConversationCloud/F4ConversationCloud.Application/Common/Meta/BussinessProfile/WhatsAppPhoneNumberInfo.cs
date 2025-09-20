using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Meta.BussinessProfile
{
    public class WhatsAppPhoneNumberInfoViewModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("verified_name")]
        public string VerifiedName { get; set; }

        [JsonPropertyName("status")]
        public string WhatsAppStatus { get; set; }
    }



}
