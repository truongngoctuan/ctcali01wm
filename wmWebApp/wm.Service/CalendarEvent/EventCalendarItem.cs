using System;

namespace wm.Service.CalendarEvent
{
    public class EventCalendarItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string classs { get; set; }
        public Int64 start { get; set; }
        public Int64 end { get; set; }
    }
}