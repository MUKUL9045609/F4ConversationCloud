namespace F4ConversationCloud.SuperAdmin.Models
{
    public class UserManagementListFilter
    {
        public string NameFilter { get; set; } = string.Empty;
        public string EmailFilter { get; set; } = string.Empty;
        public int RoleFilter { get; set; } = 0;
        public string CreatedOnFilter { get; set; } = string.Empty;
        public string UpdatedOnFilter { get; set; } = string.Empty;
        public string StatusFilter { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
