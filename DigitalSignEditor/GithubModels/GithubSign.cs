using System;
using System.Collections.Generic;
using System.Linq;
using DigitalSignEditor.Data.Models;
using Newtonsoft.Json;

namespace DigitalSignEditor.GithubModels {

    public class GithubSign {

        public GithubSign(Sign sign) {
            var slides = sign.Slides == null ? new List<Slide>() : sign.Slides.Where(s => s.IsActive && (s.StartDate == null || s.StartDate >= DateTime.Today) && (s.EndDate == null || s.EndDate <= DateTime.Today));
            Url = sign.Url ?? "";
            Title = sign.Name ?? "";
            Twitter = sign.TwitterHandle ?? "";
            College = sign.College.ToString().ToLowerInvariant();
            Slides = slides.Select(s => new GithubSlide(s)).ToList();
        }

        [JsonProperty("college")]
        public string College { get; set; }

        [JsonProperty("slides")]
        public List<GithubSlide> Slides { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}