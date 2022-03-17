using System.Collections.Generic;
using System.Linq;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.Helpers;

namespace DigitalSignEditor.ApiModels {

    public class SignGroup {

        public SignGroup() {
        }

        public SignGroup(IEnumerable<Sign> signs) {
            Items = signs.OrderBy(s => s.College).ThenBy(s => s.Name).Select(s => new SignGroupItem {
                Name = s.Name,
                Description = s.Description,
                Id = s.Id,
                College = s.College.ToString().ConvertEnum(),
                Url = s.Url
            }).ToList();
        }

        public List<SignGroupItem> Items { get; set; }
    }
}