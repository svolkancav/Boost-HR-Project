using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HR_Project.Common.Extensions
{
    public static class EnumHelper
    {
        public static SelectList GetEnumSelectList<TEnum>()
        {
            var values = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new
                {
                    Value = Convert.ToInt32(e),
                    Display = (e.GetType()
                               .GetField(e.ToString())
                               .GetCustomAttributes(typeof(DisplayAttribute), false)
                               .FirstOrDefault() as DisplayAttribute)?.Name ?? e.ToString()
                });

            return new SelectList(values, "Value", "Display");
        }

        public static string GetDisplayName(this Enum value)
        {
            var displayAttribute = value
                .GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name ?? value.ToString();
        }


    }
}
