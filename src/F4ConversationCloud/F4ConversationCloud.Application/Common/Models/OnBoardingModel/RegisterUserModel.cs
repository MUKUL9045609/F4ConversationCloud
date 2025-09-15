using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Full Name can only contain letters, spaces, and - ' characters")]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }

        public string? OTP { get; set; }

        [Required (ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        public string? FullPhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required Select please select ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        public string? BankVarificationNumber { get; set; }

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
        ErrorMessage = "Password must contain at least 1 letter, 1 number, and 1 special character.")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        
        [Compare("PassWord", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; } = true;
        public int? CreatedBy { get; set; }
        public int? ModifedBy { get; set; }

        [Required(ErrorMessage = "You must agree to the terms & conditions")]
        [MustBeTrue(ErrorMessage = "You must agree to the terms & conditions")]
        public bool TermsCondition { get; set; }
        public bool EmailOtpVerified { get; set; } = false;
        //public bool PhoneOtpVerified { get; set; } = false;

        public string Role { get; set; } = "Client";

        //public int CurrentStep { get; set; }
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //  if (CurrentStep == 2)
        //    {
                
        //        if (string.IsNullOrEmpty(PhoneNumber))
        //            yield return new ValidationResult("Phone number is required", new[] { nameof(PhoneNumber) });

                
        //        if (string.IsNullOrEmpty(Address))
        //            yield return new ValidationResult("Address is required", new[] { nameof(Address) });

        //        if (string.IsNullOrEmpty(Country))
        //            yield return new ValidationResult("Country is required", new[] { nameof(Country) });

        //    }
        //}
    }
    public class MustBeTrueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is bool b && b) return ValidationResult.Success;
            return new ValidationResult(ErrorMessage ?? "You must agree to the terms & conditions");
        }
    }
    public class UserDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string BankVarificationNumber { get; set; }

       
    }
    public class RegisterUserResponse
    {

        public int NewUserId { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}

