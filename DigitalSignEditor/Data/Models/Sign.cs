using System.Collections.Generic;

namespace DigitalSignEditor.Data.Models {

    public class Sign : BaseObject {
        public string College { get; set; }
        public string Data { get; set; }
        public string Description { get; set; }
        public int MinimumHeight { get; set; }

        public int MinimumWidth { get; set; }

        public int RatioHeight { get; set; }

        public int RatioWidth { get; set; }

        public virtual ICollection<SignItem> SignItems { get; set; }
        public virtual ICollection<SignPermission> SignPermissions { get; set; }
        public string TwitterHandle { get; set; }

        public string Url { get; set; }
    }
}