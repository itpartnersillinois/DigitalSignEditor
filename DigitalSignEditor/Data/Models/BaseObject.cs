using System;

namespace DigitalSignEditor.Data.Models {

    public abstract class BaseObject {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Name { get; set; }
    }
}