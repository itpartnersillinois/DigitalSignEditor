using System;
using System.Linq;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;

namespace DigitalSignEditor.Helpers {

    public static class ChangedHelper {

        public static bool HasSignChanged(ISignRepository signRepository) {
            var items = signRepository.Read(sr => sr.ChangeIndicators.Where(ci => ci.LastUpdated < DateTime.Now)).ToList();
            if (!items.Any()) {
                return false;
            }
            foreach (var item in items) {
                _ = signRepository.Delete(item);
            }
            return true;
        }

        public static void SignChanged(ISignRepository signRepository, DateTime? startDate, DateTime? endDate) {
            if (!startDate.HasValue && !endDate.HasValue) {
                _ = signRepository.Create(new ChangeIndicator {
                    LastUpdated = DateTime.Now,
                    IsActive = true,
                    Name = "Change"
                });
            }
            if (startDate.HasValue) {
                _ = signRepository.Create(new ChangeIndicator {
                    LastUpdated = startDate.Value,
                    IsActive = true,
                    Name = "Change"
                });
            }
            if (endDate.HasValue) {
                _ = signRepository.Create(new ChangeIndicator {
                    LastUpdated = endDate.Value,
                    IsActive = true,
                    Name = "Change"
                });
            }
        }

        public static void UpdateSignLastUpdated(ISignRepository signRepository, int signId) {
            var sign = signRepository.Read(sr => sr.Signs.First(s => s.Id == signId));
            sign.LastUpdated = DateTime.Now;
            _ = signRepository.Update(sign);
        }
    }
}