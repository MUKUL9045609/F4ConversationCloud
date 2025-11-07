using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.Meta
{
    public static class WhatsAppBusinessRequestEndpoint
    {
    
        public static Uri BaseAddress { get; private set; } = new Uri("https://graph.facebook.com/v23.0/");

        
        public static Uri GraphApiVersionBaseAddress { get; private set; } = new Uri("https://graph.facebook.com/{{api-version}}/");


        public static string CreateTemplateMessage { get; private set; } = "{{WABA-ID}}/message_templates";
        public static string GetAllTemplateMessage { get; private set; } = "{{WABA-ID}}/message_templates";
        public static string GetTemplateById { get; private set; } = "{{TEMPLATE_ID}}";
        public static string DeleteTemplateMessage { get; private set; } = "{{WABA-ID}}/message_templates?hsm_id={{HSM_ID}}&name={{NAME}}";



        public static Uri GraphDeleteApiVersionBaseAddress { get; private set; } = new Uri("https://graph.facebook.com/v23.0/message_templates?hsm_id={{hsm_id}}&name={{name}}");

        public static string TemplateID { get; private set; } = "{{TEMPLATE_ID}}";
        public static string SendMessage { get; private set; } = "{{Phone-Number-ID}}/messages";
        public static string Whatsappbusinessprofile { get; private set; } = "{{Phone-Number-ID}}/whatsapp_business_profile";
    }
}
