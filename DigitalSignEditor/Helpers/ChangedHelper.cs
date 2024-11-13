using System;
using System.Linq;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;

namespace DigitalSignEditor.Helpers {

    public static class ChangedHelper {

        public static bool HasSignChanged(ISignRepository signRepository) {
            var items = signRepository.Read(sr => sr.ChangeIndicators.Where(ci => ci.IsActive && ci.LastUpdated < DateTime.Now)).ToList();
            if (!items.Any()) {
                return false;
            }
            foreach (var item in items) {
                _ = signRepository.MakeActive(item, false);
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