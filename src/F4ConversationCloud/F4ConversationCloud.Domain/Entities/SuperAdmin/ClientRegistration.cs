using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Entities.SuperAdmin
{
    public class ClientRegistration
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganizationsName { get; set; }

        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public int Role { get; set; }
        public int RegistrationStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
