using System;

namespace DigitalSignEditor.ApiModels {

    public class QueueItem {
        public bool IsActive { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedString => LastUpdated.ToString("g");
        public string Name { get; set; }
    }
}