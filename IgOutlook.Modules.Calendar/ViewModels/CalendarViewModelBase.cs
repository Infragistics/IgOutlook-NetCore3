using IgOutlook.Business.Calendar;
using IgOutlook.Core;
using IgOutlook.Core.Dialogs;
using IgOutlook.Services;
using Infragistics.Controls.Schedules;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace IgOutlook.Modules.Calendar.ViewModels
{
    public class CalendarViewBase : NavigationAwareViewModelBase
    {
        #region Protected Properties

        protected IEventAggregator EventAggregator { get; private set; }
        protected IDialogService DialogService { get; private set; }
        protected ICalendarService CalendarService { get; private set; }
        protected ICategoryService CategoryService { get; private set; }

        #endregion //Protected Properties

        #region Public Properties

        public List<ReminderInterval> ReminderItemsSource { get { return GenerateReminderIntervals(); } }

        private OutlookCalendarViewMode _currentViewMode;
        public OutlookCalendarViewMode CurrentViewMode
        {
            get { return _currentViewMode; }
            set
            {
                SetProperty(ref _currentViewMode, value);

                RaisePropertyChanged("IsDayViewSelected");
                RaisePropertyChanged("IsWeekViewSelected");
                RaisePropertyChanged("IsWorkWeekSelected");
                RaisePropertyChanged("IsMonthViewSelected");
                RaisePropertyChanged("IsScheduleViewSelected");
            }
        }

        public bool IsDayViewSelected { get { return CurrentViewMode == OutlookCalendarViewMode.DayViewDay; } }
        public bool IsWeekViewSelected { get { return CurrentViewMode == OutlookCalendarViewMode.DayViewWeek; } }
        public bool IsWorkWeekSelected { get { return CurrentViewMode == OutlookCalendarViewMode.DayViewWorkWeek; } }
        public bool IsMonthViewSelected { get { return CurrentViewMode == OutlookCalendarViewMode.MonthView; } }
        public bool IsScheduleViewSelected
        {
            get
            {
                return (CurrentViewMode == OutlookCalendarViewMode.ScheduleViewWeek ||
                    CurrentViewMode == OutlookCalendarViewMode.ScheduleViewWorkWeek ||
                    CurrentViewMode == OutlookCalendarViewMode.ScheduleViewDay);
            }
        }

        private ActivityBase _activity;
        public ActivityBase Activity
        {
            get { return _activity; }
            set
            {
                SetProperty(ref _activity, value);

                DeleteCommand.RaiseCanExecuteChanged();
                ShowRecurrenceDialogCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand ShowRecurrenceDialogCommand { get; private set; }
        public DelegateCommand SetReminderCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }


        private XamScheduleDataManager _dataManager;
        public XamScheduleDataManager DataManager
        {
            get { return _dataManager; }
            set { SetProperty(ref _dataManager, value); }
        }

        private ReminderInterval _reminderInterval;
        public ReminderInterval ReminderInterval
        {
            get { return _reminderInterval; }
            set { SetProperty(ref _reminderInterval, value); }
        }

        #endregion //Public Properties

        #region Constructor

        public CalendarViewBase(IEventAggregator eventAggragator, IDialogService dialogService, ICalendarService calendarService, ICategoryService categoryService, XamScheduleDataManager dataManager)
        {
            DataManager = dataManager;
            EventAggregator = eventAggragator;
            DialogService = dialogService;
            CalendarService = calendarService;
            CategoryService = categoryService;

            CurrentViewMode = OutlookCalendarViewMode.DayViewWorkWeek;

            DeleteCommand = new DelegateCommand(Delete, IsActivitySelected);
            ShowRecurrenceDialogCommand = new DelegateCommand(ShowRecurrenceDialog, IsActivitySelected);

            IconSource = "pack://application:,,,/IgOutlook.Core;component/Images/Calendar.ico";
        }

        #endregion //Constructor

        #region Private Methods

        private List<ReminderInterval> GenerateReminderIntervals()
        {
            var reminders = new List<ReminderInterval>();

            reminders.Add(new ReminderInterval(null, Resources.ResourceStrings.None_Text));
            reminders.Add(new ReminderInterval(new TimeSpan(0, 0, 0), "0 " + Resources.ResourceStrings.Minutes_Text));
            reminders.Add(new ReminderInterval(new TimeSpan(0, 0, 5), "5 " + Resources.ResourceStrings.Minutes_Text));
            reminders.Add(new ReminderInterval(new TimeSpan(0, 10, 0), "10 " + Resources.ResourceStrings.Minutes_Text));
            reminders.Add(new ReminderInterval(new TimeSpan(0, 15, 0), "15 " + Resources.ResourceStrings.Minutes_Text));
            reminders.Add(new ReminderInterval(new TimeSpan(0, 30, 0), "30 " + Resources.ResourceStrings.Minutes_Text));

            reminders.Add(new ReminderInterval(new TimeSpan(1, 0, 0), "1 " + Resources.ResourceStrings.Hour_Text));

            for (int i = 2; i < 12; i++)
            {
                reminders.Add(new ReminderInterval(new TimeSpan(i, 0, 0), i + " " + Resources.ResourceStrings.Hours_Text));
            }

            reminders.Add(new ReminderInterval(new TimeSpan(12, 0, 0), "0.5 " + Resources.ResourceStrings.Days_Text));
            reminders.Add(new ReminderInterval(new TimeSpan(18, 0, 0), "18 " + Resources.ResourceStrings.Hours_Text));
            reminders.Add(new ReminderInterval(new TimeSpan(24, 0, 0), "1 " + Resources.ResourceStrings.Day_Text));
            reminders.Add(new ReminderInterval(new TimeSpan(48, 0, 0), "2 " + Resources.ResourceStrings.Days_Text));

            return reminders;
        }

        #endregion //Private Methods

        #region Commands

        private void ShowRecurrenceDialog()
        {
            DataManager.DisplayActivityRecurrenceDialog(null, Activity, Resources.ResourceStrings.ActivityRecurrence_Text, null, null);
        }

        private void Delete()
        {
            DataManager.Remove(_activity);
        }

        private bool IsActivitySelected()
        {
            return Activity != null;
        }

        #endregion //Commands
    }
}
