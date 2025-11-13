using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace F4ConversationCloud.SuperAdmin.Handler
{
    public class MessageBodyValidationAttribute : ValidationAttribute
    {
        // Regex to match variables like {{1}}, {{22}}, ...
        private static readonly Regex VarRegex = new Regex(@"{{\d+}}", RegexOptions.Compiled);

        // Regex to strip HTML tags (CKEditor output)
        private static readonly Regex HtmlTagRegex = new Regex(@"<[^>]*>", RegexOptions.Compiled);

        // Normalize whitespace: spaces, tabs, newlines, non-breaking spaces
        private static string NormalizeWhitespace(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // Replace &nbsp; and \u00A0 with a normal space
            input = input.Replace("&nbsp;", " ").Replace("\u00A0", " ");

            // Collapse whitespace sequences
            return Regex.Replace(input, @"\s+", " ").Trim();
        }

        // Count "normal words" (letters/digits/underscore), excluding variables and HTML
        private static int CountWordsExcludingVariables(string input)
        {
            if (string.IsNullOrEmpty(input)) return 0;

            // Strip HTML tags
            var clean = HtmlTagRegex.Replace(input, " ");

            // Remove variables
            clean = VarRegex.Replace(clean, " ");

            // Normalize whitespace
            clean = NormalizeWhitespace(clean);

            // Word tokens: letters/digits/underscore (Unicode-friendly if needed)
            // Basic tokenization works fine for most languages. For stricter Unicode, use \p{L}\p{N} with RegexOptions.
            var matches = Regex.Matches(clean, @"\b[\w]+\b");
            return matches.Count;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var content = value as string;
            if (string.IsNullOrWhiteSpace(content))
            {
                return ValidationResult.Success;
            }

            // Find all variables in the content
            var matches = VarRegex.Matches(content);
            var numbers = matches
                .Cast<Match>()
                .Select(m => int.Parse(m.Value.Replace("{", "").Replace("}", "")))
                .ToList();

            // --- Rule 1: No duplicates ---
            var uniqueNumbers = numbers.Distinct().ToList();
            if (numbers.Count != uniqueNumbers.Count)
            {
                return new ValidationResult("Duplicate variables found.");
            }

            // --- Rule 2: Sequential numbers starting from {{1}} with no gaps ---
            uniqueNumbers.Sort();
            for (int i = 0; i < uniqueNumbers.Count; i++)
            {
                if (uniqueNumbers[i] != i + 1)
                {
                    return new ValidationResult("Variables must be sequential starting from {{1}}.");
                }
            }

            // --- Rule 3: Surrounding words — at least 2 before AND 2 after (excluding other variables & HTML) ---
            foreach (Match match in matches)
            {
                int index = match.Index;
                int varLen = match.Value.Length;

                // Use substrings from the original content so indices stay correct
                string before = content.Substring(0, index);
                string after = content.Substring(index + varLen);

                int beforeCount = CountWordsExcludingVariables(before);
                int afterCount = CountWordsExcludingVariables(after);

                if (!(beforeCount >= 2 && afterCount >= 2))
                {
                    return new ValidationResult("Each variable must be surrounded by at least 2 normal words before and 2 normal words after (excluding other variables).");
                }
            }

            return ValidationResult.Success;
        }
    }
}
