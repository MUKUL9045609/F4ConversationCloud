using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public class WhatsappBusinessProfileDTO
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("profile_picture_handle")]
        public string ProfilePictureHandle { get; set; }
    }
}
