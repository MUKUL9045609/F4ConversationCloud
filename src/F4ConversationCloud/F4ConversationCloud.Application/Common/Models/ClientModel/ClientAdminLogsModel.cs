using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.ClientModel
{
    public class ClientAdminLogsModel
    {
        public string Source { get; set; }
        public DateTime LogDate { get; set; } = DateTime.UtcNow;
        public string LogType { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Data { get; set; }

        public int? SessionUserId { get; set; }
    }
}
