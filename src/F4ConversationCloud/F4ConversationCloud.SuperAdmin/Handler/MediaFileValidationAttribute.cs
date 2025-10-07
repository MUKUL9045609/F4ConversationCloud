using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.SuperAdmin.Models;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Handler
{
    public class MediaFileValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (TemplateViewModel)validationContext.ObjectInstance;

            if (model.MediaType == (int)MediaType.Location)
            {
                return ValidationResult.Success;
            }

            if (model.File == null)
            {
                return new ValidationResult("File is required for selected media type.");
            }

            var allowedExtensions = model.MediaType switch
            {
                (int)MediaType.Image => new[] { ".jpg", ".jpeg", ".png" },
                (int)MediaType.Video => new[] { ".mp4", ".avi", ".mov", ".wmv" },
                (int)MediaType.Document => new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx" },
                _ => Array.Empty<string>()
            };

            var fileExtension = Path.GetExtension(model.File.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return new ValidationResult($"Invalid file type. Allowed: {string.Join(", ", allowedExtensions)}");
            }

            return ValidationResult.Success;
        }
    }
}
