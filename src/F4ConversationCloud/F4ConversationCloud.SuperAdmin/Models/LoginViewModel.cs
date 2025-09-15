using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "This Field is Required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
