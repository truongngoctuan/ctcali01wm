using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wm.Web2.Models
{
    public class CalendarEventItemViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public Int64 start { get; set; }
        public Int64 end { get; set; }
        public string status { get; set; }

    }

    public class MonthlyEventsViewModel
    {
        public int success { get; set; }
        public IEnumerable<CalendarEventItemViewModel> result { get; set; }
    }
}