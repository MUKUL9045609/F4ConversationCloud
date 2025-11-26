using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
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

        public static string GetDisplayNameById<TEnum>(int id) where TEnum : Enum
        {
            var enumValue = (TEnum)(object)id;
            var memberInfo = typeof(TEnum).GetMember(enumValue.ToString()).FirstOrDefault();

            if (memberInfo != null)
            {
                var displayAttr = memberInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttr != null)
                {
                    return displayAttr.Name;
                }
            }

            return enumValue.ToString(); // fallback to enum name
        }

        public static bool IsValidEnumValue<T>(string value) where T : Enum
        {
            if (Enum.TryParse(typeof(T), value, ignoreCase: true, out _))
                return true;

            return GetEnumDisplayNames<T>().Any(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<string> GetEnumDisplayNames<T>() where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (!field.IsSpecialName)
                {
                    var display = field.GetCustomAttributes(typeof(DisplayAttribute), false)
                                       .Cast<DisplayAttribute>()
                                       .FirstOrDefault();

                    if (display != null)
                        yield return display.Name;
                    else
                        yield return field.Name;
                }
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}
