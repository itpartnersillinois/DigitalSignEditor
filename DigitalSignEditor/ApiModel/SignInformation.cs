using System.Collections.Generic;
using System.Linq;
using DigitalSignEditor.Data.Models;

namespace DigitalSignEditor.ApiModel {

    public class SignInformation : SignGroupItem {

        public SignInformation() {
        }

        public SignInformation(Sign sign) {
            Name = sign.Name;
            Description = sign.Description;
            Id = sign.Id;
            College = sign.College;
            Items = sign.SignItems.OrderBy(s => s.Order).Select(si => new SignInformationItem {
                Name = si.Name,
                Description = si.Description,
                Id = si.Id,
                Data = si.Data,
                EndDate = si.EndDate,
                Order = si.Order,
                SignOption = "",
                StartDate = si.StartDate
            }).ToList();
        }

        public List<SignInformationItem> Items { get; set; }
    }
}