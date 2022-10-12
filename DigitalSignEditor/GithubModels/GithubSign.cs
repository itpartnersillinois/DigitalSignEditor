using System;
using System.Collections.Generic;
using System.Linq;
using DigitalSignEditor.Data.Models;
using Newtonsoft.Json;

namespace DigitalSignEditor.GithubModels {

    public class GithubSign {

        public GithubSign(Sign sign) {
            var slides = sign.Slides == null ? new List<Slide>() :
                sign.Slides.Where(s => s.IsActive && (s.StartDate == null || DateTime.Now >= s.StartDate) && (s.EndDate == null || DateTime.Now <= s.EndDate))
                    .OrderBy(s => s.Order).ToList();
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