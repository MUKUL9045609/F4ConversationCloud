namespace F4ConversationCloud.SuperAdmin.Models
{
    public class RegisteredBusinessDetailViewModel
    {
        public int RegistrationId { get; set; }
        public string OrganisationName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public ClientManagementViewModel clientManagementViewModel { get; set; }
    }
}
