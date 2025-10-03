using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class MasterTaxViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "SGST is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "SGST must be greater than 0")]
        public decimal SGST { get; set; }
        [Required(ErrorMessage = "CGST is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "CGST must be greater than 0")]
        public decimal CGST { get; set; }
        [Required(ErrorMessage = "IGST is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "IGST must be greater than 0")]
        public decimal IGST { get; set; }
    }
}
