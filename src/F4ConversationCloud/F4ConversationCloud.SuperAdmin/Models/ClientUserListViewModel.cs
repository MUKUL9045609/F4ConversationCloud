using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class ClientUserListViewModel
    {
        public string SearchString { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public ClientUserListViewItem Columns { get; set; } = new ClientUserListViewItem();
        public IEnumerable<ClientUserListViewItem> data { get; set; } = new List<ClientUserListViewItem>();

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
        }
    }
}
