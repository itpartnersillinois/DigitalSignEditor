using System;
using System.Collections.Generic;
using System.Linq;

namespace DigitalSignEditor.Calendar {

    public class CalendarHelper {
        private Func<string, IEnumerable<dynamic>> access;

        public CalendarHelper(Func<string, IEnumerable<dynamic>> access) {
            this.access = access;
        }

        public IEnumerable<CalendarItem> Get(string url) =>
            access(url).Select(i => new CalendarItem { Start = i.start, End = i.end, Subject = i.subject }).OrderBy(ci => ci.Start).ToList();
    }
}