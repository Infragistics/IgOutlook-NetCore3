using IgOutlook.Core.Dialogs;
using IgOutlook.Core.Events;
using IgOutlook.Modules.Calendar.Events;
using IgOutlook.Services;
using Infragistics;
using Infragistics.Controls.Schedules;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace IgOutlook.Modules.Calendar.ViewModels
{
    public class CalendarViewModel : CalendarViewBase
    {
        #region Private Fields

        private Infragistics.DateRange? _selectedTimeRange;
        private ObservableCollection<IgOutlook.Business.Calendar.ResourceCalendar> _resourceCalendars;
        private CalendarClosedEvent _calendarClosedEvent;
        private ViewItemsCountChangedEvent _viewItemsCountChangedEvent;

        private OutlookDateNavigatorSelectedDatesChanged outlookDateNavigatorSelectedDatesChanged;

        #endregion //Private Fields

        #region Public Properties

        private IOutlookDateNavigator _dateNavigator;
        public IOutlookDateNavigator DateNavigator
        {
            get { return _dateNavigator; }
            set { SetProperty(ref _dateNavigator, value); }
        }

        public DelegateCommand<Infragistics.DateRange?> SelectedTimeRangeChangedCommand { get; private set; }
        public DelegateCommand NewAppointmentCommand { get; private set; }
        public DelegateCommand NewMeetingCommand { get; private set; }

        private ResourceCalendar _activeCalendar;
        public ResourceCalendar ActiveCalendar
        {
            get { return _activeCalendar; }
            set
            {
                SetProperty(ref _activeCalendar, value);

                if (_activeCalendar != null)
                    UpdateVisibleActivitiesCount();
            }
        }

        #endregion //Public Properties

        #region Base Class Overrides

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            Title = Resources.ResourceStrings.CalendarGroup_Header_Calendar;
            EventAggregator.GetEvent<ViewActivateEvent>().Publish(Title);

            if (!string.IsNullOrEmpty(navigationContext.Parameters.ToString()))
            {
                var initialCalendarId = string.Format("{0}[{1}]", navigationContext.Parameters[Parameters.ResourceIdKey], navigationContext.Parameters[Parameters.CalendarIdKey]);

                var calendarGroup = DataManager.CalendarGroups.FirstOrDefault(c => c.InitialSelectedCalendarId == initialCalendarId);

                if (calendarGroup == null)
                {
                    this.DataManager.CalendarGroups.Add(new CalendarGroup { InitialCalendarIds = initialCalendarId, InitialSelectedCalendarId = initialCalendarId });
                }
                else
                {
                    calendarGroup.Calendars[0].IsVisible = true;
                }
            }
        }

        #endregion //Base Class Overrides

        #region Constructor

        public CalendarViewModel(IEventAggregator eventAggragator, IDialogService dialogService, ICalendarService calendarService, ICategoryService categoryService, XamScheduleDataManager dataManager)
            : base(eventAggragator, dialogService, calendarService, categoryService, dataManager)
        {
            var unselectedCalendarChangedEvent = eventAggragator.GetEvent<UnselectedCalendarChangedEvent>();
            var selectedCalendarChangedEvent = eventAggragator.GetEvent<SelectedCalendarChangedEvent>();
            _calendarClosedEvent = eventAggragator.GetEvent<CalendarClosedEvent>();
            _viewItemsCountChangedEvent = EventAggregator.GetEvent<ViewItemsCountChangedEvent>();
            outlookDateNavigatorSelectedDatesChanged = EventAggregator.GetEvent<OutlookDateNavigatorSelectedDatesChanged>();
            _selectedTimeRange = SetDefaultSelectedTimeRange();

            this.DateNavigator = new OutlookDateNavigator();

            eventAggragator.GetEvent<DateNavigatorSelectedDatesChanged>().Subscribe(OnDateNavigatorSelectedDatesChanged);

            this.DateNavigator.SelectedDatesChanged += DateNavigator_SelectedDatesChanged;

            this.DataManager.DialogFactory = new CustomAppointmentDialogFactory(DialogService, eventAggragator);

            _resourceCalendars = calendarService.GetResourceCalendars();

            var listScheduleDataConnector = new ListScheduleDataConnector();
            listScheduleDataConnector.ResourceCalendarPropertyMappings = new ResourceCalendarPropertyMappingCollection { UseDefaultMappings = true };
            listScheduleDataConnector.ResourcePropertyMappings = new ResourcePropertyMappingCollection { UseDefaultMappings = true };
            listScheduleDataConnector.AppointmentPropertyMappings = new AppointmentPropertyMappingCollection { UseDefaultMappings = true };
            listScheduleDataConnector.ActivityCategoryPropertyMappings = new ActivityCategoryPropertyMappingCollection { UseDefaultMappings = true };
            listScheduleDataConnector.AppointmentPropertyMappings.MetadataPropertyMappings = new Infragistics.MetadataPropertyMappingCollection();
            listScheduleDataConnector.AppointmentPropertyMappings.MetadataPropertyMappings.Add("IsMeetingRequest", "IsMeetingRequest");
            listScheduleDataConnector.AppointmentPropertyMappings.MetadataPropertyMappings.Add("AppointmentIds", "AppointmentIds");
            listScheduleDataConnector.AppointmentPropertyMappings.MetadataPropertyMappings.Add("To", "To");
            listScheduleDataConnector.AppointmentPropertyMappings.MetadataPropertyMappings.Add("IsNewVariance", "IsNewVariance");

            listScheduleDataConnector.ResourceItemsSource = calendarService.GetResources();
            listScheduleDataConnector.ResourceCalendarItemsSource = _resourceCalendars;
            listScheduleDataConnector.AppointmentItemsSource = calendarService.GetAppointments();
            listScheduleDataConnector.ActivityCategoryItemsSource = categoryService.GetCategories();


            DataManager.DataConnector = listScheduleDataConnector;
            DataManager.CurrentUserId = "davids";
            DataManager.CalendarGroups.Add(new CalendarGroup { InitialCalendarIds = "davids[davidsCalendar]", InitialSelectedCalendarId = "davids[davidsCalendar]" });

            DataManager.ActivityRemoved += DataManager_ActivityRemoved;
            DataManager.ActivityAdded += DataManager_ActivityAdded;
            DataManager.ActivityChanging += DataManager_ActivityChanging;
            DataManager.ActivityChanged += DataManager_ActivityChanged;

            foreach (var calendar in _resourceCalendars)
            {
                calendar.PropertyChanged += OnCalendarIsVisiblePropertyChanged;
            }

            unselectedCalendarChangedEvent.Subscribe(OnUnselectedCalendarChanged);

            SelectedTimeRangeChangedCommand = new DelegateCommand<Infragistics.DateRange?>(SelectedTimeRangeChanged);
            NewAppointmentCommand = new DelegateCommand(NewAppointment);
            NewMeetingCommand = new DelegateCommand(NewMeeting);
        }

        private DateRange? SetDefaultSelectedTimeRange()
        {
            var startTime = DateTimeHelper.RoundUp(DateTime.Now, TimeSpan.FromMinutes(15));
            var endTime = startTime.AddMinutes(30);
            return new DateRange(startTime, endTime);
        }

        #endregion //Constructor

        #region Private Methods

        private void OnDateNavigatorSelectedDatesChanged(ObservableCollection<DateTime> selectedDates)
        {
            this.DateNavigator.SetSelectedDates(selectedDates.ToList());
        }

        private void UpdateVisibleActivitiesCount()
        {
            var dates = DateNavigator.GetSelectedDates();
            dates.ToList().Sort();

            if (dates.Count > 0 && ActiveCalendar != null)
            {
                if (dates[0].AddDays(dates.Count - 1).Day == dates[dates.Count - 1].Day)
                {
                    var start = new DateTime(dates[0].Year, dates[0].Month, dates[0].Day, 0, 0, 0);
                    var end = new DateTime(dates[dates.Count - 1].Year, dates[dates.Count - 1].Month, dates[dates.Count - 1].Day, 23, 59, 59);
                    var activities = DataManager.GetActivities(new ActivityQuery(ActivityTypes.All, new Infragistics.DateRange(start, end), ActiveCalendar)).Activities;
                    _viewItemsCountChangedEvent.Publish(activities.Count);
                }
            }
        }

        #endregion //Private Methods

        #region Event Handlers

        private void DataManager_ActivityChanged(object sender, ActivityChangedEventArgs e)
        {
            if (e.Activity.DataItem == null) return;

            var appointment = (IgOutlook.Business.Calendar.Appointment)e.Activity.DataItem;

            //Meetings
            if (appointment.IsMeetingRequest)
            {
                //A new Variance was created 
                if (appointment.IsNewVariance)
                {
                    appointment.IsNewVariance = false;
                    CalendarService.GenerateAssociatedVarianceAppointments(appointment);
                }
                else
                {
                    CalendarService.UpdateAssociatedAppointments(appointment);
                }
            }
        }

        private void DataManager_ActivityChanging(object sender, ActivityChangingEventArgs e)
        {
            //Meetings
            if ((bool)e.Activity.Metadata["IsMeetingRequest"])
            {
                //A new Variance was created 
                if (e.OriginalActivityData != null && e.Activity.IsOccurrence && e.OriginalActivityData.IsVariance == false)
                {
                    e.Activity.Metadata["IsNewVariance"] = true;
                }
            }
        }

        private void DataManager_ActivityAdded(object sender, ActivityAddedEventArgs e)
        {
            if (e.Activity.DataItem != null)
            {
                var appointment = (IgOutlook.Business.Calendar.Appointment)e.Activity.DataItem;

                if (appointment.IsMeetingRequest)
                {
                    CalendarService.GenerateAssociatedAppointments(appointment);
                }
            }
        }

        private void DataManager_ActivityRemoved(object sender, ActivityRemovedEventArgs e)
        {
            var appointment = (IgOutlook.Business.Calendar.Appointment)e.Activity.DataItem;

            if (e.Activity.IsVariance)
            {

                if (appointment.IsMeetingRequest)
                {
                    CalendarService.GenerateAssociatedVarianceAppointments(appointment);
                }
            }
            else
            {
                if (appointment.IsMeetingRequest)
                {
                    CalendarService.DeleteAssociatedAppointments(appointment);
                }
            }
        }

        private void OnCalendarIsVisiblePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var calendar = (IgOutlook.Business.Calendar.ResourceCalendar)sender;

            if (e.PropertyName == "IsVisible")
            {
                if (calendar.IsVisible == false)
                {
                    _calendarClosedEvent.Publish(calendar.Name);
                }
            }
        }

        private void OnUnselectedCalendarChanged(string calendarName)
        {
            var calendar = _resourceCalendars.FirstOrDefault(r => r.Name == calendarName);

            if (calendar != null)
            {
                calendar.PropertyChanged -= OnCalendarIsVisiblePropertyChanged;
                calendar.IsVisible = false;
                calendar.PropertyChanged += OnCalendarIsVisiblePropertyChanged;
            }
        }

        void DateNavigator_SelectedDatesChanged(object sender, Infragistics.Controls.Editors.SelectedDatesChangedEventArgs e)
        {
            outlookDateNavigatorSelectedDatesChanged.Publish(new ObservableCollection<DateTime>(DateNavigator.GetSelectedDates()));
            UpdateVisibleActivitiesCount();
        }

        #endregion //Event handlers

        #region Commands

        private void SelectedTimeRangeChanged(Infragistics.DateRange? dateRange)
        {
            _selectedTimeRange = dateRange;
        }

        private void NewAppointment()
        {
            if (!_selectedTimeRange.HasValue)
                _selectedTimeRange = SetDefaultSelectedTimeRange();

            CustomAppointmentDialogFactory.IsAppointmentMeetingRequest = false;

            DateTime start, end;

            if (_selectedTimeRange.Value.Start > _selectedTimeRange.Value.End)
            {
                start = _selectedTimeRange.Value.End;
                end = _selectedTimeRange.Value.Start;
            }
            else
            {
                start = _selectedTimeRange.Value.Start;
                end = _selectedTimeRange.Value.End;
            }

            var duration = new TimeSpan((end - start).Ticks);
            start = DateTimeHelper.ConvertFromLocalToUtc(start);

            DataManager.DisplayActivityDialog(ActivityType.Appointment,
                System.Windows.Application.Current.MainWindow,
                DataManager.ResourceItems[0].PrimaryCalendar,
                start,
                duration,
                false);
        }

        private void NewMeeting()
        {
            if (!_selectedTimeRange.HasValue)
                _selectedTimeRange = SetDefaultSelectedTimeRange();

            CustomAppointmentDialogFactory.IsAppointmentMeetingRequest = true;

            DateTime start, end;

            if (_selectedTimeRange.Value.Start > _selectedTimeRange.Value.End)
            {
                start = _selectedTimeRange.Value.End;
                end = _selectedTimeRange.Value.Start;
            }
            else
            {
                start = _selectedTimeRange.Value.Start;
                end = _selectedTimeRange.Value.End;
            }

            var duration = new TimeSpan((end - start).Ticks);
            start = DateTimeHelper.ConvertFromLocalToUtc(start);

            DataManager.DisplayActivityDialog(ActivityType.Appointment,
                System.Windows.Application.Current.MainWindow,
                DataManager.ResourceItems[0].PrimaryCalendar,
                start,
                duration,
                false);
        }

        #endregion //Commands
    }
}
