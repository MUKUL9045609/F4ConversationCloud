using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class BaseSuccessResponse
    {
        [JsonPropertyName("messaging_product")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
