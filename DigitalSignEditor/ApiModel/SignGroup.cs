using System.Collections.Generic;
using System.Linq;
using DigitalSignEditor.Data.Models;

namespace DigitalSignEditor.ApiModel {

    public class SignGroup {

        public SignGroup() {
        }

        public SignGroup(IEnumerable<Sign> signs) {
            Items = signs.OrderBy(s => s.College).ThenBy(s => s.Name).Select(s => new SignGroupItem {
                Name = s.Name,
                Description = s.Description,
                Id = s.Id,
                College = s.College
            }).ToList();
        }

        public List<SignGroupItem> Items { get; set; }
    }
}