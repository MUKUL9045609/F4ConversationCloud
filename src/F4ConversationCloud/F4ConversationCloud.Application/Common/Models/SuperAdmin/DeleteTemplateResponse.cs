using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class DeleteTemplateResponse
    {
        public bool success { get; set; }
        public string message { get; set; }

    }
}
