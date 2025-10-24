using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using Twilio.TwiML.Voice;

namespace F4ConversationCloud.ClientAdmin.Models.CampaignViewModel
{
    public class CreateAudientMansterGroupViewModel
    {

        public string GroupName { get; set; }

        [Required (ErrorMessage = "Sheet is required")]
        [FileExtensionValidation(new[] { ".csv" }, ErrorMessage = "Only CSV files are allowed.")]
        public IFormFile? ExcelFile { get; set; }

        public string ExelFileUrl { get; set; }
        public string ExelFileName { get; set; }
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var validator = validationContext.GetService(typeof(FileRequestValidator)) as FileRequestValidator;
        //    var task = Task.Run(() => validator?.Validate(this));
        //    return task.Result;
        //}
    }
}


public class FileExtensionValidationAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly string[] _allowedExtensions;

    public FileExtensionValidationAttribute(string[] allowedExtensions)
    {
        _allowedExtensions = allowedExtensions;
        ErrorMessage = "Invalid file type.";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file == null) return ValidationResult.Success;

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!_allowedExtensions.Contains(extension))
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-fileextension", ErrorMessage);
        context.Attributes.Add("data-val-fileextension-allowedextensions", string.Join(",", _allowedExtensions));
    }
}








