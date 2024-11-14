using System;
using System.Linq;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;

namespace DigitalSignEditor.Helpers {

    public static class ChangedHelper {

        public static bool HasSignChanged(ISignRepository signRepository) {
            var items = signRepository.Read(sr => sr.ChangeIndicators.Where(ci => ci.IsActive)).ToList();
            items = items.Where(ci => ci.LastUpdated < DateTime.Now).ToList(); // for some reason, date is not being filtered correctly in SQL
            if (!items.Any()) {
                return false;
            }
            foreach (var item in items) {
                _ = signRepository.MakeActive(item, false);
            }
            var itemsDeleted = signRepository.Read(sr => sr.ChangeIndicators.Where(ci => !ci.IsActive)).ToList();
            itemsDeleted = itemsDeleted.Where(ci => ci.LastUpdated < DateTime.Now.AddDays(-30)).ToList();
            foreach (var item in itemsDeleted) {
                _ = signRepository.Delete(item);
            }
            return true;
        }

        public static void SignChanged(ISignRepository signRepository, DateTime? startDate, DateTime? endDate, string name) {
            if (!startDate.HasValue && !endDate.HasValue) {
                _ = signRepository.Create(new ChangeIndicator {
                    IsActive = true,
                    Name = "Change: " + name
                });
            }
            if (startDate.HasValue) {
                _ = signRepository.Create(new ChangeIndicator {
                    LastUpdated = startDate.Value,
                    IsActive = true,
                    Name = "Change Schedule Start: " + name
                }, false);
            }
            if (endDate.HasValue) {
                _ = signRepository.Create(new ChangeIndicator {
                    LastUpdated = endDate.Value,
                    IsActive = true,
                    Name = "Change Schedule End: " + name
                }, false);
            }
        }

        public static void UpdateSignLastUpdated(ISignRepository signRepository, int signId) {
            var sign = signRepository.Read(sr => sr.Signs.First(s => s.Id == signId));
            sign.LastUpdated = DateTime.Now;
            _ = signRepository.Update(sign);
        }
    }
}