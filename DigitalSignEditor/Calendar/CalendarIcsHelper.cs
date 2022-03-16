using System;
using System.Collections.Generic;
using System.Linq;

namespace DigitalSignEditor.Calendar {

    public class CalendarIcsHelper {
        private Func<string, string> access;

        public CalendarIcsHelper(Func<string, string> access) {
            this.access = access;
        }

        public IEnumerable<CalendarItem> Get(string url) => IcsTranslator.Translate(access(url)).OrderBy(c => c.Start).ToList();
    }
}