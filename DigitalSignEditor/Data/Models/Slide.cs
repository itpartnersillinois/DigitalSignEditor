using System;

namespace DigitalSignEditor.Data.Models {

    public class Slide : BaseObject {
        public string Data { get; set; }
        public string Description { get; set; }
        public DateTime? EndDate { get; set; }
        public SlideType Option { get; set; }
        public int Order { get; set; }
        public int SignId { get; set; }
        public DateTime? StartDate { get; set; }
        public int? StorageItemId { get; set; }
        public string Url { get; set; }
    }
}