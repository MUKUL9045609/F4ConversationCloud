using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class CreateUpdateClientRegistrationViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [RegularExpression(@"^[a-zA-Z]+(?:'[a-zA-Z]+)?$", ErrorMessage = "Please enter valid First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [RegularExpression(@"^[a-zA-Z]+(?:'[a-zA-Z]+)?$", ErrorMessage = "Please enter valid Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required]
        [EmailAddress(ErrorMessage = "Please enter valid Email")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        [Required]
        public int Role { get; set; }

        [Display(Name = "Role")]
        public string? RoleName { get; set; }
    }
}
