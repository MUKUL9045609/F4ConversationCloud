using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.OnBoardingModel
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "This Field is Required")]
        [Display(Name = "User Name")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string EmailId { get; set; }
    }
    public class ConfirmPasswordViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Display(Name = "Password")]
        [Required]
        [Compare("ConfirmPassword", ErrorMessage = " ")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[._@$!%*?&#])[A-Za-z\\d._@$!%*?&#]{8,}$", ErrorMessage = " ")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[._@$!%*?&#])[A-Za-z\\d._@$!%*?&#]{8,}$", ErrorMessage = "Password should be atleast 8 Character long and it should contain atleast one Uppercase. one lowercase, one special charactor and one number")]
        public string ConfirmPassword { get; set; }
    }
}
