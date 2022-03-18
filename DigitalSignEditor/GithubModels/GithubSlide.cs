using DigitalSignEditor.Data.Models;
using Newtonsoft.Json;

namespace DigitalSignEditor.GithubModels {

    public class GithubSlide {

        public GithubSlide(Slide slide) {
            Filename = slide.DisplayUrl ?? "";
            Filename = slide.DisplayUrl ?? "";
            Type = slide.Option.ToString().ToLowerInvariant();
        }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("filenameCompressed")]
        public string FilenameCompressed { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}