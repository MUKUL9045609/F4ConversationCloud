using System.ComponentModel.DataAnnotations;

namespace BuldanaUrban.Onboarding.Models
{
    public class UserDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string BankVarificationNumber { get; set; }
        public string PassWord { get; set; }
        public bool OtpVerified { get; set; } 
        public string Role { get; set; } 
    }
}
