using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net;
using Twilio.Types;
namespace F4ConversationCloud.ClientAdmin.Models.CampaignViewModel
{
    public class CreateCampaignViewModel
    {
        public int CampaignId { get; set; }
        public bool? IsEdit { get; set; }
        public bool? Isdraft { get; set; }

        public string CampaignName { get; set; }


        public DateTime? FromDate { get; set; }

        [DateGreaterThan("FromDate", ErrorMessage = "To Date cannot be earlier than From Date.")]
        public DateTime? ToDate { get; set; }
        public string CurrentTab { get; set; } = "Preview";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CurrentTab == "Preview")
            {
                if (FromDate == null && ToDate == null)
                {
                    if (FromDate == null)
                    {
                        yield return new ValidationResult("From Date is required.", new[] { nameof(CampaignName) });
                    }
                    if (ToDate == null)
                    {
                        yield return new ValidationResult("To Date is required.", new[] { nameof(CampaignName) });
                    }
                }
            
            }
            else if (CurrentTab == "details")
            {
                if (string.IsNullOrWhiteSpace(CampaignName))
                {
                    yield return new ValidationResult("Campaign name is required.", new[] { nameof(CampaignName) });
                }


            }
        }
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
