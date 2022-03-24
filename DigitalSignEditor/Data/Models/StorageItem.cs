using System;
using System.Text.RegularExpressions;

namespace DigitalSignEditor.Data.Models {

    public class StorageItem : BaseObject {

        public StorageItem() {
        }

        public StorageItem(string name, byte[] data) {
            Name = new Regex("[^a-zA-Z0-9 .]").Replace(name, string.Empty).Replace(" ", "_");
            Data = data;
            IsActive = true;
            LastUpdated = DateTime.Now;
        }

        public byte[] Data { get; set; }
    }
}