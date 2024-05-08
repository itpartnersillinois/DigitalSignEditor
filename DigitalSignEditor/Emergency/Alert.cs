using System;

namespace DigitalSignEditor.Emergency {

    public class Alert {

        public Alert() {
            Time = $"Last Updated: {DateTime.Now:g}";
            LastUpdated = DateTime.Now;
        }

        public string Description { get; set; }
        public bool IsSafe => string.IsNullOrWhiteSpace(ResponseType) || ResponseType.Equals("AllClear", StringComparison.OrdinalIgnoreCase) || ResponseType.Equals("None", StringComparison.OrdinalIgnoreCase);
        public DateTime LastUpdated { get; set; }
        public string ResponseType { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
    }
}