using IgOutlook.Core;
using IgOutlook.Core.Behaviors;
using IgOutlook.Modules.Calendar.Converters;
using IgOutlook.Modules.Calendar.TabItems;
using Infragistics.Controls.Schedules;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace IgOutlook.Modules.Calendar.Views
{
    /// <summary>
    /// Interaction logic for CalendarView.xaml
    /// </summary>
    [DependentView(typeof(HomeTab), RegionNames.RibbonTabRegion)]
    public partial class CalendarView : ISupportDataContext
    {
        public CalendarView()
        {
            InitializeComponent();

            //Name="ocv" CurrentViewMode="{Binding CurrentViewMode, Mode=TwoWay}" DateNavigator="{Binding DateNavigator}"
            //DataManager = "{Binding DataManager}" ActiveCalendar = "{Binding ActiveCalendar, Mode=OneWayToSource}"
            XamOutlookCalendarView view = new XamOutlookCalendarView();
            view.Name = "ocv";

            Binding commandBinding = new Binding();
            commandBinding.Source = DataContext;
            commandBinding.Path = new PropertyPath("SelectedTimeRangeChangedCommand");

            EventToCommand eventToCommand = new EventToCommand();
            eventToCommand.EventArgsConverter = new XamOutlookCalendarViewSelectedTimeRangeChangedConverter();
            BindingOperations.SetBinding(eventToCommand, EventToCommand.CommandProperty, commandBinding);

            var triggers = Interaction.GetTriggers(view);
            var eventTrigger = new System.Windows.Interactivity.EventTrigger() { EventName = "SelectedTimeRangeChanged" };
            eventTrigger.Actions.Add(eventToCommand);
            triggers.Add(eventTrigger);

            Binding currentViewModeBinding = new Binding();
            currentViewModeBinding.Source = DataContext;
            currentViewModeBinding.Path = new System.Windows.PropertyPath("CurrentViewMode");
            currentViewModeBinding.Mode = BindingMode.TwoWay;
            view.SetBinding(XamOutlookCalendarView.CurrentViewModeProperty, currentViewModeBinding);

            Binding dateNavigatorBinding = new Binding();
            dateNavigatorBinding.Source = DataContext;
            dateNavigatorBinding.Path = new System.Windows.PropertyPath("DateNavigator");
            view.SetBinding(XamOutlookCalendarView.DateNavigatorProperty, dateNavigatorBinding);

            Binding dataManagerBinding = new Binding();
            dataManagerBinding.Source = DataContext;
            dataManagerBinding.Path = new System.Windows.PropertyPath("DataManager");
            view.SetBinding(XamOutlookCalendarView.DataManagerProperty, dataManagerBinding);

            Binding activeCalendarBinding = new Binding();
            activeCalendarBinding.Source = DataContext;
            activeCalendarBinding.Path = new System.Windows.PropertyPath("ActiveCalendar");
            activeCalendarBinding.Mode = BindingMode.OneWayToSource;
            view.SetBinding(XamOutlookCalendarView.ActiveCalendarProperty, activeCalendarBinding);

            _placeHolder.Children.Add(view);
        }
    }
}
