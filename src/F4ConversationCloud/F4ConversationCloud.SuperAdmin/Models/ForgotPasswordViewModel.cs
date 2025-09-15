using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "This Field is Required")]
        [Display(Name = "User Name")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string UserName { get; set; }
    }
}
