using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Common {

    /// <summary>
    /// EnumExtentions: EnumのDisplayName取得用
    /// </summary>
    public static class EnumExtentions {
        public static string DisplayName(this Enum value) {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes != null) {
                return descriptionAttributes[0].Name;
            } else {
                return value.ToString();
            }
        }
    }
}