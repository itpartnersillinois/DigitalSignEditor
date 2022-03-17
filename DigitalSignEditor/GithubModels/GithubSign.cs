using System;
using System.Collections.Generic;
using System.Linq;
using DigitalSignEditor.Data.Models;

namespace DigitalSignEditor.GithubModels {

    public class GithubSign {

        public GithubSign(Sign sign) {
            var slides = sign.Slides.Where(s => s.IsActive && (s.StartDate == null || s.StartDate >= DateTime.Today) && (s.EndDate == null || s.EndDate <= DateTime.Today));
            Url = sign.Url ?? "";
            Title = sign.Name ?? "";
            Twitter = sign.TwitterHandle ?? "";
            College = sign.College.ToString().ToLowerInvariant();
            Slides = sign == null ? new List<GithubSlide>() : slides.Select(s => new GithubSlide(s)).ToList();
        }

        public string College { get; set; }

        public List<GithubSlide> Slides { get; set; }
        public string Title { get; set; }
        public string Twitter { get; set; }
        public string Url { get; set; }
    }
}