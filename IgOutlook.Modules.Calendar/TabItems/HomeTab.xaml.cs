using IgOutlook.Core;
using Infragistics.Controls;
using Infragistics.Controls.Schedules;
using Infragistics.Controls.Schedules.Primitives;

namespace IgOutlook.Modules.Calendar.TabItems
{
    /// <summary>
    /// Interaction logic for HomeTab.xaml
    /// </summary>
    public partial class HomeTab : ISupportDataContext
    {
        public HomeTab()
        {
            InitializeComponent();

            SetResourceReference(StyleProperty, typeof(Infragistics.Windows.Ribbon.RibbonTabItem));

            var dayCommands = Commanding.GetCommands(_day);
            XamOutlookCalendarViewCommandSource daySource = new XamOutlookCalendarViewCommandSource()
            {
                CommandType = XamOutlookCalendarViewCommand.SwitchToDayView,
                EventName = "Click",
                TargetName = "ocv"
            };
            dayCommands.Add(daySource);

            var workWeekCommands = Commanding.GetCommands(_workWeek);
            XamOutlookCalendarViewCommandSource workweekSource = new XamOutlookCalendarViewCommandSource()
            {
                CommandType = XamOutlookCalendarViewCommand.SwitchToWorkWeekView,
                EventName = "Click",
                TargetName = "ocv"
            };
            workWeekCommands.Add(workweekSource);

            var weekCommands = Commanding.GetCommands(_week);
            XamOutlookCalendarViewCommandSource weekSource = new XamOutlookCalendarViewCommandSource()
            {
                CommandType = XamOutlookCalendarViewCommand.SwitchToFullWeekView,
                EventName = "Click",
                TargetName = "ocv"
            };
            weekCommands.Add(weekSource);

            var monthCommands = Commanding.GetCommands(_month);
            XamOutlookCalendarViewCommandSource monthSource = new XamOutlookCalendarViewCommandSource()
            {
                CommandType = XamOutlookCalendarViewCommand.SwitchToMonthView,
                EventName = "Click",
                TargetName = "ocv"
            };
            monthCommands.Add(monthSource);

            var scheduleCommands = Commanding.GetCommands(_schedule);
            XamOutlookCalendarViewCommandSource scheduleSource = new XamOutlookCalendarViewCommandSource()
            {
                CommandType = XamOutlookCalendarViewCommand.SwitchToScheduleView,
                EventName = "Click",
                TargetName = "ocv"
            };
            scheduleCommands.Add(scheduleSource);
        }
    }
}
