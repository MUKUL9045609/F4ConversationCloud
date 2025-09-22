using F4ConversationCloud.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [RegularExpression(@"^[a-zA-Z\s']+$", ErrorMessage = "Full Name can only contain letters And spaces")]
        [NoWhitespace( ErrorMessage = "Name Cannot contain Empty Spaces.")]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }

        public string? OTP { get; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Enter Valid Phone Number")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must be 10 digits and not start with 1-5.")]
        public string PhoneNumber { get; set; }

        public string? FullPhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

      //  [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
        ErrorMessage = "Password must contain at least 1 letter, 1 number, and 1 special character.")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [Compare("PassWord", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; } = true;

        [MustBeTrue(ErrorMessage = "You must agree to the terms & conditions")]
        public bool TermsCondition { get; set; }
        public bool EmailOtpVerified { get; set; } = false;
        public string Role { get; set; } = "Client";

        [Required(ErrorMessage = "Select Time Zones")]
        public string Timezone { get; set; }
        public ClientFormStage Stage { get; set; }
        public int UserId { get; set; }

        public IEnumerable<TimeZoneResponse> TimeZones { get; set; } = new List<TimeZoneResponse>();
    }
    public class MustBeTrueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is bool b && b) return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? "You must agree to the terms & conditions");
        }
    }
    public class NoWhitespaceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string str && string.IsNullOrWhiteSpace(str))
            {
                return new ValidationResult("The " + validationContext.DisplayName + " field cannot contain only whitespace.");
            }
            return ValidationResult.Success;
        }
    }


    public class UserDetailsViewModel
    {
        public string FirstName { get; }
        public string LastName { get;  }
        public string Email { get;  }
        public string PhoneNumber { get;  }
        public string Address { get;  }
        public string Country { get;  }
        public string TimeZone { get;  }
        public string Role { get; }
        public bool TermsCondition { get; set; }
        public ClientFormStage Stage { get; }
        public int UserId { get; set; }

    }
    public class RegisterUserResponse
    {

        public int NewUserId { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class Loginrequest {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }


        //[MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [Required(ErrorMessage = "Password is required")]
        //[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
       // [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
     //   ErrorMessage = "Password must contain at least 1 letter, 1 number, and 1 special character.")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }


    }

    public class LoginViewModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }        
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ClientFormStage Stage { get; set; }
        public string Password { get; set; }

    }
    public class LoginResponse
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public LoginViewModel Data { get; set; }
    }

    public class TimeZoneResponse {

        public string name { get; set; }
        public string current_utc_offset { get; set; }
    }
}

