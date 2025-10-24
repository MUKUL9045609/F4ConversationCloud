using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class ClientRegistrationListViewModel
    {
        public string NameFilter { get; set; } = string.Empty;
        public string EmailFilter { get; set; } = string.Empty;
        public string ContactNumberFilter { get; set; } = string.Empty;
        public int RoleFilter { get; set; } = 0;
        public string CreatedOnFilter { get; set; } = string.Empty;
        public string UpdatedOnFilter { get; set; } = string.Empty;
        public string RegistrationStatusFilter { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public ClientRegistrationListViewItem Columns { get; set; } = new ClientRegistrationListViewItem();
        public IEnumerable<ClientRegistrationListViewItem> data { get; set; } = new List<ClientRegistrationListViewItem>();
        public IEnumerable<SelectListItem> RolesList { get; set; }

        public class ClientRegistrationListViewItem
        {
            public int Id { get; set; }

            [Display(Name = "Sr. No.")]
            public int SrNo { get; set; }

            [Display(Name = "User Name")]
            public string Name { get; set; }

            [Display(Name = "Email Address")]
            public string Email { get; set; }

            [Display(Name = "Contact Number")]
            public string ContactNumber { get; set; }

            [Display(Name = "Role")]
            public int Role { get; set; }

            [Display(Name = "Role")]
            public string RoleName { get; set; }
            [Display(Name = "Registration Status")]
            public int RegistrationStatus { get; set; }

            [Display(Name = "Created Date & Time")]
            public DateTime CreatedOn { get; set; }

            [Display(Name = "Last Updated")]
            public DateTime? UpdatedOn { get; set; }
        }
    }
}
