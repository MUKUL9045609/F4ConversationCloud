using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.ClientModel
{
    public class AudienceGroupMasterModel
    {
        public int  Id{ get; set; }
        public string GroupName { get; set; }
        public string ExelFileUrl { get; set; }
        public string ExelFileName { get; set; }
        public int ClientInfoId { get; set; }
        public bool CreatedBy { get; set; }
        public bool UpdatedBy { get; set; }
    }
}
