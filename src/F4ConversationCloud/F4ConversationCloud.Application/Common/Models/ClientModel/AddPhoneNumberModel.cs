namespace F4ConversationCloud.Application.Common.Models.ClientModel
{
    public class AddPhoneNumberModel
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public string WABAId { get; set; }
        public string BusinessId { get; set; }
        public string BusinessCategory { get; set; }
        public string WhatsAppDisplayName { get; set; }
        public string PhoneNumberId { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
    }
}
