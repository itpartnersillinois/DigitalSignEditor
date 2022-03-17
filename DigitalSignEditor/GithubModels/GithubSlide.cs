using DigitalSignEditor.Data.Models;

namespace DigitalSignEditor.GithubModels {

    public class GithubSlide {

        public GithubSlide(Slide slide) {
            Filename = slide.Url ?? "";
            Type = slide.Option.ToString().ToLowerInvariant();
        }

        public string Filename { get; set; }
        public string Type { get; set; }
    }
}