using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class UserListViewModel
    {
        public string NameFilter { get; set; } = string.Empty;
        public string EmailFilter { get; set; } = string.Empty;
        public int RoleFilter { get; set; } = 0;
        public string CreatedOnFilter { get; set; } = string.Empty;
        public string UpdatedOnFilter { get; set; } = string.Empty;
        public string StatusFilter { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public UserListViewItem Columns { get; set; } = new UserListViewItem();
        public IEnumerable<UserListViewItem> data { get; set; } = new List<UserListViewItem>();
        public IEnumerable<SelectListItem> RolesList { get; set; }

        public class UserListViewItem
        {
            public int Id { get; set; }

            [Display(Name = "Sr. No.")]
            public int SrNo { get; set; }

            [Display(Name = "User Name")]
            public string Name { get; set; }

            [Display(Name = "Email Address")]
            public string Email { get; set; }

            [Display(Name = "Mobile No.")]
            public string MobileNo { get; set; }

            [Display(Name = "Role")]
            public int Role { get; set; }

            [Display(Name = "Role")]
            public string RoleName { get; set; }

            [Display(Name = "Designation")]
            public string Designation { get; set; }

            [Display(Name = "IP Address")]
            public string IPAddress { get; set; }
            [Display(Name = "Status")]
            public bool IsActive { get; set; }

            [Display(Name = "Created Date & Time")]
            public DateTime CreatedOn { get; set; }

            [Display(Name = "Last Updated")]
            public DateTime? UpdatedOn { get; set; }
        }
    }
}
