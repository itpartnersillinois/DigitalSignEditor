using System;

namespace DigitalSignEditor.Data.Models {

    public class SignItem : BaseObject {
        public string Data { get; set; }
        public string Description { get; set; }
        public DateTime? EndDate { get; set; }
        public SignType Option { get; set; }
        public int Order { get; set; }
        public int SignId { get; set; }
        public DateTime? StartDate { get; set; }
        public int? StorageItemId { get; set; }
        public string Url { get; set; }
    }
}