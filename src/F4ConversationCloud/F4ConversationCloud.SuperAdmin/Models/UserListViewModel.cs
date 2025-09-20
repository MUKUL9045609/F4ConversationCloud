using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class UserListViewModel
    {
        public string SearchString { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public UserListViewItem Columns { get; set; } = new UserListViewItem();
        public IEnumerable<UserListViewItem> data { get; set; } = new List<UserListViewItem>();

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

            public bool IsActive { get; set; }

            [Display(Name = "Created Date & Time")]
            public DateTime CreatedOn { get; set; }

            [Display(Name = "Last Updated")]
            public DateTime? UpdatedOn { get; set; }
        }
    }
}
