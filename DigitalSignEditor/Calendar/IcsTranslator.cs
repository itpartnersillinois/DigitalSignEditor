using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DigitalSignEditor.Calendar {

    public static class IcsTranslator {
        public const int numberToPull = 10;

        public static IEnumerable<CalendarItem> Translate(string icsFile) {
            var returnValue = new List<CalendarItem>();
            DateTime? startDate = null;
            DateTime? endDate = null;
            var subject = "";
            foreach (var line in icsFile.Split('\n').Select(s => s.Trim(new char[] { '\r', ' ' }))) {
                if (string.IsNullOrWhiteSpace(subject)) {
                    subject = ParseLine(line, "SUMMARY;ENCODING=QUOTED-PRINTABLE:");
                }
                if (startDate == null) {
                    startDate = ParseDate(line, "DTSTART:");
                }
                if (endDate == null) {
                    endDate = ParseDate(line, "DTEND:");
                }
                if (line.Equals("END:VEVENT")) {
                    if (startDate.HasValue && endDate.HasValue && !string.IsNullOrWhiteSpace(subject)) {
                        returnValue.Add(new CalendarItem {
                            Start = startDate.Value,
                            End = endDate.Value,
                            Subject = subject
                        });
                    }
                    subject = "";
                    startDate = null;
                    endDate = null;
                    if (returnValue.Count >= numberToPull) {
                        return returnValue;
                    }
                }
            }
            return returnValue;
        }

        private static DateTime? ParseDate(string s, string header) {
            var item = ParseLine(s, header);
            if (string.IsNullOrEmpty(item)) {
                return null;
            }
            var date = DateTime.ParseExact(item.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture);
            if (date < DateTime.Now.Date) {
                return null;
            }
            var time = DateTime.ParseExact(item.Substring(9, 6), "HHmmss", CultureInfo.InvariantCulture);
            return date.Date + time.TimeOfDay;
        }

        private static string ParseLine(string s, string header) {
            return s.StartsWith(header) ? s.Replace(header, "").Trim() : string.Empty;
        }
    }
}