using System.Text.RegularExpressions;

namespace F4ConversationCloud.Domain.Helpers
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            string[] words = input.Trim().Split(' ');
            if (words.Length == 0) return "";
            string result = words[0].ToLowerInvariant();
            for (int i = 1; i < words.Length; i++)
                if (!string.IsNullOrEmpty(words[i]))
                    result += char.ToUpperInvariant(words[i][0]) + words[i].Substring(1).ToLowerInvariant();
            return Regex.Replace(result, "[^a-zA-Z0-9]", "");
        }

        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var preserveWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
              SpecialCasedWords.AC.GetDisplayName(), SpecialCasedWords.AMT.GetDisplayName(),SpecialCasedWords.DB.GetDisplayName()
            };



            string[] words = input.Trim().Split(' ').Where(w => !string.IsNullOrEmpty(w)).ToArray();

            if (words.Length == 0) return "";

            return string.Join(" ", words.Select(w => preserveWords.Contains(w) ? w.ToUpperInvariant() : char.ToUpperInvariant(w[0]) + w.Substring(1).ToLowerInvariant()));
        }

        public static string CleanString(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.Replace("\r\n", " ")
                         .Replace("\n", " ")
                         .Replace("\r", " ")
                         .Trim();

            if (input.EndsWith(","))
            {
                input = input.Substring(0, input.Length - 1).TrimEnd();
            }

            return input;
        }
    }
}
