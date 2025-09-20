using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class CreateUpdateUserViewModel
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

        [Display(Name = "Mobile No.")]
        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid MobileNo.")]
        public string MobileNo { get; set; }

        [Display(Name = "Password")]
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[._@$!%*?&#])[A-Za-z\\d._@$!%*?&#]{8,}$", ErrorMessage = "Password should be atleast 8 Character long and it should contain atleast one Uppercase. one lowercase, one special charactor and one number")]
        public string Password { get; set; }

        public string IPAddress { get; set; }

        [Display(Name = "Role")]
        [Required]
        public int Role { get; set; }

        [Display(Name = "Role")]
        public string? RoleName { get; set; }

        [Display(Name = "Designation")]
        [Required]
        public string Designation { get; set; }
    }
}
