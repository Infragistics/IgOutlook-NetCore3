using IgOutlook.Core;
using IgOutlook.Modules.Calendar.Views;
using IgOutlook.Services;
using Infragistics.Controls.Schedules;
using Prism.Ioc;

namespace IgOutlook.Modules.Calendar
{
    public class CalendarModule : ModuleBase
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CalendarView>("CalendarView");
            containerRegistry.RegisterForNavigation<AppointmentView>("AppointmentView");
            containerRegistry.RegisterForNavigation<MeetingView>("MeetingView");

            containerRegistry.RegisterSingleton<ICalendarService, CalendarService>();
            containerRegistry.RegisterSingleton<ICategoryService, CategoryService>();

            containerRegistry.RegisterInstance(new XamScheduleDataManager());
        }

        protected override void ResolveOutlookGroup()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.OutlookBarGroupRegion, typeof(OutlookGroups.CalendarGroup));
        }
    }
}
