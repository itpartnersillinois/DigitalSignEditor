using System.Collections.Generic;
using System.Linq;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.Helpers;

namespace DigitalSignEditor.ApiModels {

    public class SignInformation : SignGroupItem {

        public SignInformation() {
        }

        public SignInformation(Sign sign) {
            Name = sign.Name;
            Description = sign.Description;
            Id = sign.Id;
            Url = sign.Url;
            College = sign.College.ToString().ConvertEnum();
            LastUpdated = sign.LastUpdated.ToString("g");
            MaximumSize = sign.MaximumSize + "KB";
            MinimumWidth = sign.MinimumWidth + "px";
            MinimumHeight = sign.MinimumHeight + "px";
            Ratio = sign.RatioWidth + ":" + sign.RatioHeight;
            SignType = sign.SignType.ToString();
            Twitter = string.IsNullOrWhiteSpace(sign.TwitterHandle) ? "None" : sign.TwitterHandle;
            Items = sign.Slides == null ? new List<SlideInformation>() : sign.Slides.OrderBy(s => s.Order).ThenBy(s => s.Name).Select(si => new SlideInformation {
                Name = si.Name,
                Description = string.IsNullOrWhiteSpace(si.Description) ? "" : si.Description,
                Id = si.Id,
                IsActive = si.IsActive,
                Data = si.Data,
                EndDate = si.EndDate.HasValue ? si.EndDate.Value.ToString("g") : "N/A",
                Order = si.Order,
                SignOption = (int) si.Option,
                StartDate = si.StartDate.HasValue ? si.StartDate.Value.ToString("g") : "N/A",
                Url = string.IsNullOrWhiteSpace(si.DisplayUrl) ? "/Image/" + si.StorageItemId : si.DisplayUrl
            }).ToList();
        }

        public List<SlideInformation> Items { get; set; }
        public string LastUpdated { get; set; }
        public string MaximumSize { get; set; }
        public string MinimumHeight { get; set; }
        public string MinimumWidth { get; set; }
        public string Ratio { get; set; }

        public string Twitter { get; set; }
    }
}