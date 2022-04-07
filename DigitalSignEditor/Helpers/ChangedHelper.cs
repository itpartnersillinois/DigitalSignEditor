using System;
using System.Linq;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;

namespace DigitalSignEditor.Helpers {

    public static class ChangedHelper {

        public static bool HasSignChanged(ISignRepository signRepository) {
            var items = signRepository.Read(sr => sr.ChangeIndicators).ToList();
            if (!items.Any()) {
                return false;
            }
            foreach (var item in items) {
                _ = signRepository.Delete(item);
            }
            return true;
        }

        public static void SignChanged(ISignRepository signRepository) {
            var items = signRepository.Read(sr => sr.ChangeIndicators);
            if (!items.Any()) {
                _ = signRepository.Create(new ChangeIndicator {
                    LastUpdated = DateTime.Now,
                    IsActive = true,
                    Name = "Change"
                });
            }
        }
    }
}