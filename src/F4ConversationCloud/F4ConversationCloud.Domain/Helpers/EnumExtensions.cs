using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BuldanaUrban.Domain.Helpers
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this System.Enum value) where TAttribute : Attribute
        {
            var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            return memberInfo?.GetCustomAttribute<TAttribute>();
        }

        public static string GetDisplayName(this System.Enum value)
        {
            var attribute = value.GetAttribute<DisplayAttribute>();
            return attribute?.Name ?? value.ToString();
        }
    }
}
