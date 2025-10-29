using F4ConversationCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates
{
    public class WhatsAppResponse
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("contacts")]
        public List<Contact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }
    }

    public class Contact
    {
        [JsonPropertyName("input")]
        public string Input { get; set; }

        [JsonPropertyName("wa_id")]
        public string WaId { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
