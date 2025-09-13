using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Entities
{
    public class RegisteredUsers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string PassWord  { get; set; }
        public bool  IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int ModifedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime  ModifiedOn { get; set; }

    }
}
