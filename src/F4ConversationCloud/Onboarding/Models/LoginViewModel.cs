using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.Onboarding.Models
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }


        //[MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [Required(ErrorMessage = "Password is required")]
        //[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        // [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
        //   ErrorMessage = "Password must contain at least 1 letter, 1 number, and 1 special character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
