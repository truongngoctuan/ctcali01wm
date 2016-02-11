using System;
using wm.Model;

namespace wm.Service.CalendarEvent
{
    public class CalendarEventItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public Int64 start { get; set; }
        public Int64 end { get; set; }
        public string status { get; set; }
        public OrderStatus orderStatus { get; set; }

    }
}