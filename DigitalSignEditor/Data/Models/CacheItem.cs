namespace DigitalSignEditor.Data.Models {

    public class CacheItem : BaseObject {

        public CacheItem() {
        }

        public string Data { get; set; }

        public int MinutesUntilCacheExpires { get; set; }
    }
}