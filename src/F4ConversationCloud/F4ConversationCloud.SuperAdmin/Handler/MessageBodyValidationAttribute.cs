using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace F4ConversationCloud.SuperAdmin.Handler
{
    public class MessageBodyValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var content = value as string;
            if (string.IsNullOrWhiteSpace(content))
                return ValidationResult.Success;

            var regex = new Regex(@"{{\d+}}");
            var matches = regex.Matches(content);

            var numbers = matches.Select(m => int.Parse(m.Value.Replace("{", "").Replace("}", ""))).ToList();
            var uniqueNumbers = numbers.Distinct().ToList();

            if (numbers.Count != uniqueNumbers.Count)
                return new ValidationResult("Duplicate variables found.");

            uniqueNumbers.Sort();
            for (int i = 0; i < uniqueNumbers.Count; i++)
            {
                if (uniqueNumbers[i] != i + 1)
                    return new ValidationResult("Variables must be sequential starting from {{1}}.");
            }

            foreach (Match match in matches)
            {
                int index = match.Index;
                string before = content.Substring(0, index).Trim();
                string after = content.Substring(index + match.Value.Length).Trim();

                var beforeWords = before.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                var afterWords = after.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if ((beforeWords.Length + afterWords.Length) < 3)
                    return new ValidationResult("Each variable must be surrounded by at least 3 words combined before and after.");
            }

            return ValidationResult.Success;
        }
    }
}
