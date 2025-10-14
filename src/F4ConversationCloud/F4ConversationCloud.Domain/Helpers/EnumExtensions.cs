using Microsoft.AspNetCore.Mvc.Rendering;
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

        public static List<SelectListItem> ToSelectList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .Select(e => new SelectListItem
                       {
                           Value = Convert.ToInt32(e).ToString(),
                           Text = e.GetType()
                                   .GetMember(e.ToString())
                                   .First()
                                   .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
                       })
                       .ToList();
        }

        public static TAttribute Get<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }


    }
}
