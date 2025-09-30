using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class MasterPriceViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Conversation Type is required")]
        public int ConversationType { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "From Date is required.")]
        public DateTime? FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public IEnumerable<SelectListItem>? ConversationTypeList { get; set; }
        public List<MasterPrice>? MasterPrices { get; set; }
        public MasterPriceFilterModel? MasterPriceFilterModel { get; set; }
    }
}
