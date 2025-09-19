using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Entities.SuperAdmin
{
    public class Auth
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public string MobileNo { get; set; }
    }
}
