namespace F4ConversationCloud.Application.Common.Models.ClientModel
{
    public class AddPhoneNumberListFilter
    {
        public string WABAIdFilter { get; set; } = string.Empty;
        public string BusinessIdFilter { get; set; } = string.Empty;
        public string BusinessCategoryFilter { get; set; } = string.Empty;
        public string WhatsappDisplayNameFilter { get; set; } = string.Empty;
        public string PhoneNumberIdFilter { get; set; } = string.Empty;
        public string PhoneNumberFilter { get; set; } = string.Empty;
        public string StatusFilter { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
