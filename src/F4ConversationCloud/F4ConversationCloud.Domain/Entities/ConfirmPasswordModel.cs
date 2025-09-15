using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Entities
{
    public class ConfirmPasswordModel
    {
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}
