using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class ClientRegistrationListItemModel
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public int Role { get; set; }
        public string RoleName { get; set; }
        public int RegistrationStatus { get; set; }
        public string OrganizationsName { get; set; }
        public string Category { get; set; }
        public string ClientId { get; set; }
        public bool IsActive { get; set; }
         public int AccountStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int Stage { get; set; }
    }
}
