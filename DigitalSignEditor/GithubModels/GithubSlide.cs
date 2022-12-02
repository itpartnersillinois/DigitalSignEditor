using DigitalSignEditor.Data.Models;
using Newtonsoft.Json;

namespace DigitalSignEditor.GithubModels {

    public class GithubSlide {

        public GithubSlide(Slide slide) {
            Data = slide.Data ?? "";
            Filename = slide.Url ?? "";
            FilenameFull = slide.DisplayUrl ?? "";
            FilenameCompressed = slide.DisplayUrlCompressed ?? "";
            Title = slide.Name;
            Type = slide.Option.ToString().ToLowerInvariant();
        }

        [JsonProperty("data")]
        public string Data{ get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("filenameCompressed")]
        public string FilenameCompressed { get; set; }

        [JsonProperty("filenameFull")]
        public string FilenameFull { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}