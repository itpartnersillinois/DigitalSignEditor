using System;
using System.Collections.Generic;
using System.Linq;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.Helpers;

namespace DigitalSignEditor.ApiModels {

    public class SignGroup {

        public SignGroup() {
        }

        public static IEnumerable<SignGroup> Build(IEnumerable<Sign> signs) {
            return signs.OrderBy(s => s.College).ThenBy(s => s.Name).Select(s => new SignGroupItem {
                Name = s.Name,
                Description = s.Description,
                Id = s.Id,
                College = s.College.ToString().ConvertEnum(),
                Url = s.Url,
                SignType = s.SignType.ToString(),
                NumberActiveSlides = s.Slides == null ? 0 : s.Slides.Count(s => s.IsActive && (s.StartDate == null || DateTime.Now >= s.StartDate) && (s.EndDate == null || DateTime.Now <= s.EndDate))
            }).GroupBy(sgi => sgi.College).Select(group => new SignGroup { Items = group.ToList(), CollegeName = group.Key}).ToList();
        }

        public List<SignGroupItem> Items { get; set; }

        public string CollegeName { get; set; }


    }
}