using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm.Service.CalendarEvent
{
    public class EventColorStatus
    {
        public static string NotStarted { get { return "event-info"; } }
        public static string Started { get { return "event-important"; } }
        public static string StaffConfirmed { get { return "event-important"; } }
        public static string ManagerConfirmed { get { return "event-important"; } }

        public static string Finished { get { return "event-success"; } }
    }
}
