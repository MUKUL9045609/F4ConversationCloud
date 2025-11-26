using F4ConversationCloud.Application.Common.Helper;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Onboarding.Models
{
  public class RegisterUserViewModel
        {
            //[Required(ErrorMessage = "Full Name is required")]
            //[RegularExpression(@"^[a-zA-Z\s']+$", ErrorMessage = "Full Name can only contain letters And spaces")]
            //[Remote(action: "IsValidNoWhitespace", controller: "Validation", ErrorMessage = "Name Cannot contain Empty Spaces.")]
            public string? FirstName { get; set; }

            public string? LastName { get; set; }

            //[Required(ErrorMessage = "Email is required")]
            //[EmailAddress(ErrorMessage = "Enter a valid email")]
            public string? Email { get; set; }

            [Required(ErrorMessage = "OTP is required")]
            [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must be a 6-digit number")]
            public string OTP { get; set; }

            //[Required(ErrorMessage = "Phone number is required")]
            //[RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must be 10 digits and not start with 1-5.")]
            public string PhoneNumber { get; set; }

            public string CountryCode { get; set; }

            [Required(ErrorMessage = "Address is required ")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Country is required")]
            public string? Country { get; set; }


            [Required(ErrorMessage = "Please select a state.")]
            public string StateId { get; set; }


            [Required(ErrorMessage = "Please select a city.")]
            public string CityId { get; set; }

            [Required(ErrorMessage = "Please enter a zip code.")]
            [RegularExpression(@"^\d{5,6}$", ErrorMessage = "Please enter a valid 5 or 6 digit zip code.")]
            public string ZipCode { get; set; }
            
            
            public string? OptionalAddress { get; set; }

            //[Required(ErrorMessage = "Organization Name is required. ")]
            public string OrganizationsName { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
            ErrorMessage = "Password should be at least 8 characters long and it should contain at least one Uppercase, one lowercase, one special character, and one number")]
            [DataType(DataType.Password)]
            public string PassWord { get; set; }


            [Compare("PassWord", ErrorMessage = "Passwords do not match.")]
            [DataType(DataType.Password)]
            [Required(ErrorMessage = "Confirm Password is required")]   
            public string ConfirmPassword { get; set; }
            public bool IsActive { get; set; } = true;

            [Remote(action: "IsValidTermsCondition", controller: "Validation", ErrorMessage = "Please Accept Terms and Conditions.")]
            public bool TermsCondition { get; set; }

            //[Remote(action: "IsPhoneNumberOTPVerify", controller: "Validation", AdditionalFields = "PhoneNumber", ErrorMessage = "OTP must be verified.")]
            public bool PhoneNumberOtpVerified { get; set; } = false;
            public ClientRole Role { get; set; }

            [Required(ErrorMessage = "Select Time Zones")]
            public string Timezone { get; set; }
            public ClientFormStage Stage { get; set; }
            public int UserId { get; set; }
     

             public IEnumerable<TimeZoneResponse> TimeZones { get; set; } = new List<TimeZoneResponse>();
            public IEnumerable<Cities> Cities { get; set; } = new List<Cities>();
            public IEnumerable<States> States { get; set; } = new List<States>();
            
    }
    

}

