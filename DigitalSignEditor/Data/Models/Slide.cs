using System;
using Newtonsoft.Json;

namespace DigitalSignEditor.Data.Models {

    public class Slide : BaseObject {
        public string Data { get; set; }
        public string Description { get; set; }
        public DateTime? EndDate { get; set; }
        public SlideType Option { get; set; }
        public int Order { get; set; }

        [JsonIgnore]
        public virtual Sign Sign { get; set; }

        [JsonIgnore]
        public int SignId { get; set; }

        public DateTime? StartDate { get; set; }
        public int? StorageItemId { get; set; }
        public string Url { get; set; }

        public void AssignUrl(string url) {
            this.StorageItemId = null;
            this.Url = url;
        }
    }
}