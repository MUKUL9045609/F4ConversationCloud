using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public class TemplateListForSync
    {
        public long TemplateId { get; set; }
        public int ClientInfoId { get; set; }
        public string WABAID { get; set; }
    }
}
