using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public class RegistrationModel
    {
        // Step 1: Account Type
        [Required(ErrorMessage = "Account type is required.")]
        public string AccountType { get; set; } = "Individual"; // Default to Individual as per screenshot

        // Step 2: Registration
        [Required(ErrorMessage = "Full name is required.")]
        [Display(Name = "Your Fullname")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [Display(Name = "Create Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "You must agree to terms.")]
        public bool AgreeToTerms { get; set; }

        // Step 3: Profile
        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Your Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [Display(Name = "Country of Residence")]
        public string Country { get; set; }

        // Step 4: Bank Verification
        [Required(ErrorMessage = "BVN is required.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "BVN must be 11 digits.")]
        [Display(Name = "Bank Verification Number (BVN)")]
        public string BVN { get; set; }
    }
}
