using System.Text.RegularExpressions;

namespace DigitalSignEditor.Data.Models {

    public class Donor : BaseObject {
        public string Description { get; set; }

        public string Html => string.IsNullOrWhiteSpace(Description) ? "" : "<p>" + Description.Replace("\n\r", "</p><p>").Replace("\r\n", "</p><p>").Replace("\r\r", "</p><p>").Replace("\r", "</p><p>").Replace("\n\n", "</p><p>").Replace("\n", "</p><p>") + "</p>";
        public byte[] Image { get; set; }

        public string ImageUrl => Image != null && Image.Length > 0 ? "https://digitalsigneditor/image/donor/" + Id : "";
        public string PersonName { get; set; }
        public string Url => string.IsNullOrWhiteSpace(Name) ? "" : Regex.Replace(Name.Replace(" ", "-"), "[^A-Za-z0-9-]", "").ToLowerInvariant();
    }
}