using AutoMapper;

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
            //this.CreateMap<CalendarEventItem, CalendarEventItemViewModel>();
        }
    }
}