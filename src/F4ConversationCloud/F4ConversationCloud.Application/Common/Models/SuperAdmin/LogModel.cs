using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class LogModel
    {
        public string Source { get; set; }
        public DateTime LogDate { get; set; } = DateTime.UtcNow;
        public string LogType { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string AdditionalInfo { get; set; }
        public string LoggedBy { get; set; }
    }
}
