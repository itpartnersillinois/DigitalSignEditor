using System;

namespace DigitalSignEditor.Calendar {

    public class CalendarItem {

        public string DateString => Start.Date == End.Date ?
            $"{Start.ToLocalTime():MMM dd h:mm} {Start.ToLocalTime().ToString("tt").ToLowerInvariant()} - {End.ToLocalTime():h:mm} {End.ToLocalTime().ToString("tt").ToLowerInvariant()}" :
            $"{Start.ToLocalTime():MMM dd h:mm} {Start.ToLocalTime().ToString("tt").ToLowerInvariant()} - {End.ToLocalTime():MMM dd h:mm} {End.ToLocalTime().ToString("tt").ToLowerInvariant()}";

        public DateTime End { get; set; }
        public bool IsLong => this.Subject.Length > 50;
        public DateTime Start { get; set; }
        public string Subject { get; set; }
    }
}