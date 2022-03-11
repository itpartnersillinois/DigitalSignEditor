using System.Collections.Generic;
using System.Linq;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.Helpers;

namespace DigitalSignEditor.ApiModel {

    public class SignInformation : SignGroupItem {

        public SignInformation() {
        }

        public SignInformation(Sign sign) {
            Name = sign.Name;
            Description = sign.Description;
            Id = sign.Id;
            College = sign.College.ToString().ConvertEnum();
            MinimumWidth = sign.MinimumWidth + "px";
            MinimumHeight = sign.MinimumHeight + "px";
            Ratio = sign.RatioWidth + ":" + sign.RatioHeight;
            Twitter = string.IsNullOrWhiteSpace(sign.TwitterHandle) ? "None" : sign.TwitterHandle;
            Items = sign.SignItems.OrderBy(s => s.Order).ThenBy(s => s.Name).Select(si => new SignInformationItem {
                Name = si.Name,
                Description = string.IsNullOrWhiteSpace(si.Description) ? "" : si.Description,
                Id = si.Id,
                Data = si.Data,
                EndDate = si.EndDate.HasValue ? si.EndDate.Value.ToString("d") : "N/A",
                Order = si.Order,
                SignOption = (int) si.Option,
                StartDate = si.StartDate.HasValue ? si.StartDate.Value.ToString("d") : "N/A",
                Url = string.IsNullOrWhiteSpace(si.Url) ? "/Image/" + si.StorageItemId : si.Url
            }).ToList();
        }

        public List<SignInformationItem> Items { get; set; }

        public string MinimumHeight { get; set; }
        public string MinimumWidth { get; set; }
        public string Ratio { get; set; }

        public string Twitter { get; set; }
    }
}