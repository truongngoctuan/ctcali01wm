using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wm.Service.CalendarEvent;
using wm.Web2.Models;

namespace wm.Web2.App_Start
{
    internal class AutoMapperWebProfiler : Profile
    {
        protected override void Configure()
        {
            EventConfiguration();
        }

        private void EventConfiguration()
        {
            this.CreateMap<CalendarEventItem, CalendarEventItemViewModel>();
        }
    }
}