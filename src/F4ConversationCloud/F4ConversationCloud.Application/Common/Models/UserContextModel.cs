using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public class UserContextModel
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsCreateTemplate { get; set; } = false;
        public bool IsEditTemplate { get; set; } = false;
        public bool IsDeleteTemplate { get; set; } = false;
        public bool IsView { get; set; } = false;
    }
}
