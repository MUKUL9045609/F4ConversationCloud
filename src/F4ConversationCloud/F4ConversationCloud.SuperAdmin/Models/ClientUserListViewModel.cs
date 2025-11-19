using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class ClientUserListViewModel
    {
        public string BusinessFilter { get; set; } = string.Empty;
        public string NameFilter { get; set; } = string.Empty;
        public string EmailFilter { get; set; } = string.Empty;
        public int RoleFilter { get; set; } = 0;
        public string CreatedOnFilter { get; set; } = string.Empty;
        public string UpdatedOnFilter { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public ClientUserListViewItem Columns { get; set; } = new ClientUserListViewItem();
        public IEnumerable<ClientUserListViewItem> data { get; set; } = new List<ClientUserListViewItem>();
        public IEnumerable<SelectListItem> RolesList { get; set; }

        public class ClientUserListViewItem
        {
            public int Id { get; set; }

            [Display(Name = "Sr. No.")]
            public int SrNo { get; set; }

            [Display(Name = "User Name")]
            public string Name { get; set; }

            [Display(Name = "Email Address")]
            public string Email { get; set; }

            [Display(Name = "Role")]
            public string Role { get; set; }

            [Display(Name = "Account State")]
            public bool IsActive { get; set; }

            [Display(Name = "Created Date & Time")]
            public DateTime CreatedOn { get; set; }

            [Display(Name = "Last Updated")]
            public DateTime? UpdatedOn { get; set; }

            [Display(Name = "Client Name")]
            public string BusinessName { get; set; }
            [Display(Name = "Category")]
            public string Category { get; set; }
            [Display(Name = "ClientId")]
            public string ClientId { get; set; }
        }
    }
}
