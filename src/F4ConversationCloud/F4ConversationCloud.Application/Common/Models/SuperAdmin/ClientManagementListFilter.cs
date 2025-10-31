namespace F4ConversationCloud.SuperAdmin.Models
{
    public class ClientManagementListFilter
    {
        public string ClientNameSearch { get; set; } = string.Empty;
        public string OnboardingOnFilter { get; set; } = string.Empty;
        public string StatusFilter { get; set; } = string.Empty;
        public string PhoneNumberFilter { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int RegistrationId { get; set; } = 0;
    }
}
