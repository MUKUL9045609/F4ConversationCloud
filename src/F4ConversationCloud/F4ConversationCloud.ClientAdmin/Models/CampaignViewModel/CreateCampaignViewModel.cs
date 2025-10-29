using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
namespace F4ConversationCloud.ClientAdmin.Models.CampaignViewModel
{
    public class CreateCampaignViewModel
    {
        public int CampaignId { get; set; }
        public bool? IsEdit { get; set; }
        public bool? Isdraft { get; set; }

        [Required(ErrorMessage = "Campaign name is required.")]
        public string CampaignName { get; set; }


        [Required(ErrorMessage = "From Date is required.")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required.")]
        [DateGreaterThan("FromDate", ErrorMessage = "To Date cannot be earlier than From Date.")]
        public DateTime ToDate { get; set; }

        public string NextTabName { get; set; }
    }


    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _fromDate;

        public DateGreaterThanAttribute(string fromDate)
        {
            _fromDate = fromDate;
            ErrorMessage = "Date must be later than From Date.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var fromDate = validationContext.ObjectType.GetProperty(_fromDate);
            if (fromDate == null) return ValidationResult.Success;

            var fromdate = fromDate.GetValue(validationContext.ObjectInstance) as DateTime?;
            var todate = value as DateTime?;

            if (todate.HasValue && fromdate.HasValue && todate.Value < fromdate.Value)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-dategreaterthan", ErrorMessage);
            context.Attributes.Add("data-val-dategreaterthan-other", _fromDate);
        }
    }

}
