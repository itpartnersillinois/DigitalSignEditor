using System;

namespace DigitalSignEditor.Helpers {

    public static class TextHelper {

        public static DateTime? ConvertDate(this string s) {
            DateTime returnValue;
            if (DateTime.TryParse(s, out returnValue)) {
                return returnValue;
            }
            return null;
        }

        public static string ConvertEnum(this string s) => s.Replace("_", " ");
    }
}