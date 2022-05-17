using System;
using System.Linq;

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

        public static int ConvertStringToInt(this string s) => int.Parse(string.Concat(s.Where(ch => char.IsDigit(ch))));

        public static Tuple<int, int> ConvertStringToInts(this string s) => new Tuple<int, int>(int.Parse(s[..s.IndexOf(':', StringComparison.Ordinal)]), int.Parse(s[(s.IndexOf(':', StringComparison.Ordinal) + 1)..]));

        public static string ConvertTwitterString(this string s) => s == "None" ? string.Empty : s;
    }
}