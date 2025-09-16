using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
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
