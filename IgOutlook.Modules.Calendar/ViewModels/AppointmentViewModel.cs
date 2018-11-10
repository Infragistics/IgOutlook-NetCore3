using IgOutlook.Core.Dialogs;
using IgOutlook.Modules.Calendar.Converters;
using IgOutlook.Modules.Calendar.Events;
using IgOutlook.Modules.Calendar.Resources;
using IgOutlook.Services;
using Infragistics;
using Infragistics.Controls.Schedules;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Windows;

namespace IgOutlook.Modules.Calendar.ViewModels
{
    public class AppointmentViewModel : CalendarViewBase, IDialogAware
    {
        private IMessageBoxService _messageBoxService;
        private ActivityBase _originalActivity;
        protected string AppointmetTypeName;        

        #region Public Properties

        public DelegateCommand SaveAndCloseCommand { get; private set; }
        public DelegateCommand DeleteAndCloseCommand { get; private set; }
        public DelegateCommand ToggleTimeZoneChoosersVisibilityCommand { get; private set; }

        private List<DateTime> _endTimeIntervals;
        public List<DateTime> EndTimeIntervals
        {
            get { return _endTimeIntervals; }
            set { SetProperty(ref _endTimeIntervals, value); }
        }

        private List<DateTime> _startTimeIntervals;
        public List<DateTime> StartTimeIntervals
        {
            get { return _startTimeIntervals; }
            set { SetProperty(ref _startTimeIntervals, value); }
        }

        private Visibility _timeZoneChoosersVisibility;
        public Visibility TimeZoneChoosersVisibility
        {
            get { return _timeZoneChoosersVisibility; }
            set { SetProperty(ref _timeZoneChoosersVisibility, value); }
        }
        public bool IsNewActivity { get; private set; }

        #endregion //Public Properties

        #region Constructor

        public AppointmentViewModel(IEventAggregator eventAggragator, IDialogService dialogService, ICalendarService calendarService, ICategoryService categoryService, IMessageBoxService messageBoxService, XamScheduleDataManager dataManager)
            : base(eventAggragator, dialogService, calendarService, categoryService, dataManager)
        {
            _messageBoxService = messageBoxService;
            var activityAddedEvent = eventAggragator.GetEvent<ActivityChangedEvent>();
            var converter = new ActivityUtcToLocalTimeConverter();

            TimeZoneChoosersVisibility = Visibility.Collapsed;

            activityAddedEvent.Subscribe(OnActivityChanged);

            SaveAndCloseCommand = new DelegateCommand(SaveAndClose);
            DeleteAndCloseCommand = new DelegateCommand(DeleteAndClose);
            ToggleTimeZoneChoosersVisibilityCommand = new DelegateCommand(ToggleTimeZoneChoosersVisibility);

            AppointmetTypeName = ResourceStrings.Appointment_Text;
        }

        #endregion //Constructor

        #region Commmands

        private void DeleteAndClose()
        {
            if (!IsNewActivity)
                base.DeleteCommand.Execute();

            RequestClose();
        }

        private void SaveAndClose()
        {
            SaveChanges();

            _closeRequested = true;

            RequestClose?.Invoke();
        }

        private void ToggleTimeZoneChoosersVisibility()
        {
            TimeZoneChoosersVisibility = TimeZoneChoosersVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        #endregion //Commmands

        #region Private Methods

        protected void CloseTheDialog()
        {
            RequestClose();
        }

        private void SaveChanges()
        {
            //Handle selection of reminder if any
            if (ReminderInterval != null && ReminderInterval.TimeInterval.HasValue)
            {
                Activity.Reminder = new Reminder();
                Activity.ReminderEnabled = true;
                Activity.ReminderInterval = ReminderInterval.TimeInterval.Value;
            }
            else
            {
                Activity.Reminder = null;
                Activity.ReminderEnabled = false;
                Activity.ReminderInterval = new TimeSpan();
            }

            DataManager.EndEdit(Activity);

        }

        private List<DateTime> GenerateStartTimeIntervals(DateTime startTime)
        {
            var intervalStep = new TimeSpan(0, 30, 0);
            var startTimeLocal = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, startTime.Second, DateTimeKind.Utc).ToLocalTime();
            var startInterval = new DateTime(startTime.Year, startTime.Month, startTime.Day, 0, 0, 0);
            var day = startTimeLocal.Day;

            var intervals = new List<DateTime>();

            while (startInterval.Day == day)
            {
                intervals.Add(startInterval);
                startInterval = startInterval.Add(intervalStep);
            }

            if (!intervals.Contains(startTimeLocal))
                intervals.Add(startTimeLocal);

            intervals.Sort();

            return intervals;
        }

        private List<DateTime> GenerateEndTimeIntervals(DateTime endTime)
        {
            var intervalStep = new TimeSpan(0, 30, 0);
            var endTimeLocal = new DateTime(endTime.Year, endTime.Month, endTime.Day, endTime.Hour, endTime.Minute, endTime.Second, DateTimeKind.Utc).ToLocalTime();
            var roundedStart = Round(endTimeLocal.TimeOfDay, new TimeSpan(0, 30, 0), MidpointRounding.AwayFromZero);
            var startInterval = new DateTime(endTime.Year, endTime.Month, endTime.Day, roundedStart.Hours, roundedStart.Minutes, roundedStart.Seconds);
            var duration = new TimeSpan();

            var intervals = new List<DateTime>();

            while (duration.Hours < 23)
            {
                duration = duration.Add(intervalStep);
                intervals.Add(startInterval);
                startInterval = startInterval.Add(intervalStep);
            }

            if (!intervals.Contains(endTimeLocal))
                intervals.Add(endTimeLocal);

            intervals.Sort();

            return intervals;
        }

        public TimeSpan Round(TimeSpan time, TimeSpan roundingInterval, MidpointRounding roundingType)
        {
            return new TimeSpan(
                Convert.ToInt64(Math.Round(
                    time.Ticks / (decimal)roundingInterval.Ticks,
                    roundingType
                )) * roundingInterval.Ticks
            );
        }

        public virtual void OnActivityChanged(ActivityBase act)
        {

            if (_originalActivity != null)
                return;

            _originalActivity = act;
            Activity = DataManager.BeginEditWithCopy(_originalActivity, true, out DataErrorInfo dataErrorInfo);

            //Generate Time Intervals
            StartTimeIntervals = GenerateStartTimeIntervals(Activity.Start);
            EndTimeIntervals = GenerateEndTimeIntervals(Activity.End);

            base.UpdateTitleOnPropertyChanged(Activity, "Subject", " - " + AppointmetTypeName, ResourceStrings.Untitled_Text);

            //Handle IsTimeZoneNeutral
            base.HookOnPropertyChanged(Activity, "IsTimeZoneNeutral", () =>
            {
                if (Activity.IsTimeZoneNeutral)
                {
                    Activity.Start = new DateTime(Activity.Start.Year, Activity.Start.Month, Activity.Start.Day, 0, 0, 0);
                    Activity.End = new DateTime(Activity.Start.Year, Activity.Start.Month, Activity.Start.Day, 0, 0, 0).AddDays(1);
                }
                else
                {
                    if (_originalActivity.IsTimeZoneNeutral)
                    {
                        var workingHours = DataManager.GetWorkingHours(Activity.OwningResource, Activity.Start);

                        if (workingHours.Count > 0)
                        {
                            var start = new DateTime(Activity.Start.Year, Activity.Start.Month, Activity.Start.Day, workingHours[0].Start.Hours, workingHours[0].Start.Minutes, workingHours[0].Start.Seconds);
                            var end = new DateTime(Activity.Start.Year, Activity.Start.Month, Activity.Start.Day, workingHours[0].Start.Hours, workingHours[0].Start.Minutes + 30, workingHours[0].Start.Seconds);

                            StartTimeIntervals = GenerateStartTimeIntervals(start);
                            EndTimeIntervals = GenerateEndTimeIntervals(end);

                            Activity.Start = start;
                            Activity.End = end;
                        }
                    }
                    else
                    {
                        Activity.Start = _originalActivity.Start;
                        Activity.End = _originalActivity.End;
                    }
                }
            });

            bool wasDescriptionDeafultRtfLoaded = false;

            IsNewActivity = Activity.DataItem == null;
            RaisePropertyChanged("IsNewActivity");

            Activity.PropertyChanged += (s, a) =>
            {
                if (a.PropertyName == "Description" && wasDescriptionDeafultRtfLoaded == false)
                {
                    if (IsNewActivity)
                        Title = ResourceStrings.Untitled_Text + " - " + AppointmetTypeName;
                    else
                        Title = Activity.Subject + " - " + AppointmetTypeName;

                    wasDescriptionDeafultRtfLoaded = true;
                    return;
                }

                _isDirty = true;
            };
        }

        #endregion //Private Methods

        #region Base Class Overrides

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            var activityId = navigationContext.Parameters[Parameters.ActivityIdKey];
        }

        #endregion //Base Class Overrides

        #region IDialogAware

        private bool _closeRequested;
        private bool _isDirty;

        public bool CanCloseDialog()
        {
            if (_closeRequested)
                return true;
            if (!_isDirty)
                return true;

            var interactionResult = _messageBoxService.Show("IG Outlook", ResourceStrings.SaveChangesMessage_Text, MessageBoxButtons.YesNoCancel);

            if (interactionResult == InteractionResult.Cancel)
            {
                return false;
            }
            else if (interactionResult == InteractionResult.No)
            {
                DataManager.CancelEdit(Activity, out DataErrorInfo dataErrorInfo);
                return true;
            }
            else
            {
                SaveChanges();
                return true;
            }
        }

        public event Action RequestClose;

        protected virtual void CloseDialog()
        {
            if (RequestClose != null)
            {
                _closeRequested = true;
                RequestClose();
            }
        }

        #endregion //IDialogAware
    }
}
