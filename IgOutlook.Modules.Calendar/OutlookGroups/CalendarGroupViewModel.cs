using IgOutlook.Business;
using IgOutlook.Core;
using IgOutlook.Core.Commands;
using IgOutlook.Modules.Calendar.Events;
using IgOutlook.Services;
using Infragistics.Controls.Schedules;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;

namespace IgOutlook.Modules.Calendar.OutlookGroups
{
    public class CalendarGroupViewModel : ViewModelBase
    {
        #region Protected Members

        protected ICalendarService CalendarService { get; private set; }
        protected IEventAggregator EventAggregator { get; private set; }

        #endregion //Protected Members

        #region Private Fields
        
        private bool _canExecuteDateNavigatorSelectedDatesCommand;
        private UnselectedCalendarChangedEvent _unselectedCalendarChangedEvent;
        private SelectedCalendarChangedEvent _selectedCalendarChangedEvent;
        private CalendarClosedEvent _calendarClosedEvent;
        private DateNavigatorSelectedDatesChanged _dateNavigatorSelectedDatesChanged;
        private OutlookDateNavigatorSelectedDatesChanged _outlookDateNavigatorSelectedDatesChanged;

        #endregion //Private Fields

        #region Public Properties

        public DelegateCommand<object> CalendarUnselectedCommand { get; set; }
        public DelegateCommand<ObservableCollection<DateTime>> DateNavigatorSelectedDatesCommand { get; set; }

        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        private object _selectedDates;
        public object SelectedDates
        {
            get { return _selectedDates; }
            set { SetProperty(ref _selectedDates, value); }
        }

        private object _unselectedCalendar;
        public object UnselectedCalendar
        {
            get { return _unselectedCalendar; }
            set { SetProperty(ref _unselectedCalendar, value); }
        }

        private object _selectedCalendar;
        public object SelectedCalendar
        {
            get { return _selectedCalendar; }
            set { SetProperty(ref _selectedCalendar, value); }
        }

        private XamScheduleDataManager _dataManager;
        public XamScheduleDataManager DataManager
        {
            get { return _dataManager; }
            set { SetProperty(ref _dataManager, value); }
        }

        private ObservableCollection<NavigationItem> _items;
        public ObservableCollection<NavigationItem> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private ObservableCollection<object> _selectedDataItems;
        public ObservableCollection<object> SelectedDataItems
        {
            get { return _selectedDataItems; }
            set { SetProperty(ref _selectedDataItems, value); }
        }

        #endregion //Public Properties

        #region Constructor

        public CalendarGroupViewModel(XamScheduleDataManager dataManager, ICalendarService calendarService, IEventAggregator eventAggragator, IApplicationCommands applicationCommands)
        {
            CalendarUnselectedCommand = new DelegateCommand<object>(CalendarUnselectedCommandExecuted);
            DateNavigatorSelectedDatesCommand = new DelegateCommand<ObservableCollection<DateTime>>(OnDateNavigatorSelectedDates);

            CalendarService = calendarService;
            EventAggregator = eventAggragator;

            ApplicationCommands = applicationCommands;

            DataManager = dataManager;

            GenerateMenu();

            _unselectedCalendarChangedEvent = EventAggregator.GetEvent<UnselectedCalendarChangedEvent>();
            _selectedCalendarChangedEvent = eventAggragator.GetEvent<SelectedCalendarChangedEvent>();
            _selectedCalendarChangedEvent.Subscribe(OnSelectedCalendarChanged);

            _calendarClosedEvent = EventAggregator.GetEvent<CalendarClosedEvent>();
            _calendarClosedEvent.Subscribe(OnCalendarClosed);

            _dateNavigatorSelectedDatesChanged = EventAggregator.GetEvent<DateNavigatorSelectedDatesChanged>();

            _outlookDateNavigatorSelectedDatesChanged = EventAggregator.GetEvent<OutlookDateNavigatorSelectedDatesChanged>();
            _outlookDateNavigatorSelectedDatesChanged.Subscribe(OnOutlookDateNavigatorSelectedDatesChanged);
            _canExecuteDateNavigatorSelectedDatesCommand = true;
        }

        #endregion //Constructor

        #region Commands

        private void CalendarUnselectedCommandExecuted(object obj)
        {
            if (obj == null) return;

            _unselectedCalendarChangedEvent.Publish(((NavigationItem)obj).Caption);
        }

        private void OnDateNavigatorSelectedDates(ObservableCollection<DateTime> selectedDates)
        {
            if (_canExecuteDateNavigatorSelectedDatesCommand)
                _dateNavigatorSelectedDatesChanged.Publish(selectedDates);
        }

        #endregion //Commands

        #region Private Methods

        private void OnOutlookDateNavigatorSelectedDatesChanged(ObservableCollection<DateTime> selectedDates)
        {
            _canExecuteDateNavigatorSelectedDatesCommand = false;
            SelectedDates = selectedDates;
            _canExecuteDateNavigatorSelectedDatesCommand = true;
        }

        private void OnCalendarClosed(string obj)
        {
            var item = FindNavigationItem(Items, obj);

            if (item != null)
                UnselectedCalendar = item;
        }

        private void OnSelectedCalendarChanged(string obj)
        {
            var item = FindNavigationItem(Items, obj);

            if (item != null)
                SelectedCalendar = item;
        }

        private void GenerateMenu()
        {
            Items = new ObservableCollection<NavigationItem>();

            var root1 = new NavigationItem() { Caption = Resources.ResourceStrings.CalendarGroup_MyCalendars_Text, NavigationPath = "CalendarView", CanNavigate = false };

            foreach (var resourceCalendars in CalendarService.GetUserResourceCalendars("davids"))
            {
                root1.Items.Add(new NavigationItem() { Caption = resourceCalendars.Name, DataItem = resourceCalendars, NavigationPath = CreateNavigationPath(resourceCalendars.OwningResourceId, resourceCalendars.Id) });
            }

            var root2 = new NavigationItem() { Caption = Resources.ResourceStrings.CalendarGroup_SharedCalendars_Text, NavigationPath = "CalendarView", CanNavigate = false };

            foreach (var resourceCalendars in CalendarService.GetSharedResourceCalendars())
            {
                root2.Items.Add(new NavigationItem() { Caption = resourceCalendars.Name, DataItem = resourceCalendars, NavigationPath = CreateNavigationPath(resourceCalendars.OwningResourceId, resourceCalendars.Id) });
            }

            var root3 = new NavigationItem() { Caption = Resources.ResourceStrings.CalendarGroup_TeamCalendars_Text, NavigationPath = "CalendarView", CanNavigate = false };

            foreach (var resourceCalendars in CalendarService.GetTeamResourceCalendars("davids"))
            {
                root3.Items.Add(new NavigationItem() { Caption = resourceCalendars.Name, DataItem = resourceCalendars, NavigationPath = CreateNavigationPath(resourceCalendars.OwningResourceId, resourceCalendars.Id) });
            }

            Items.Add(root1);
            Items.Add(root2);
            Items.Add(root3);
        }

        private static NavigationItem FindNavigationItem(ObservableCollection<NavigationItem> items, string caption)
        {
            NavigationItem target = null;

            System.Action<NavigationItem, object> find = (n, dItem) =>
            {
                if (n.Items.Count > 0)
                {
                    foreach (var item in n.Items)
                    {
                        if (target != null) return;

                        if (item.Caption == caption)
                        {
                            target = item;
                        }
                    }
                }
            };

            foreach (var ni in items)
            {
                if (ni.Caption == caption)
                {
                    return ni;
                }
            }

            foreach (var ni in items)
            {
                if (target == null)
                    find(ni, caption);
            }

            return target;
        }

        private string CreateNavigationPath(string reourceId, string calendarId)
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(Parameters.ResourceIdKey, reourceId);
            parameters.Add(Parameters.CalendarIdKey, calendarId);
            return "CalendarView" + parameters.ToString();
        }

        #endregion //Private Methods
    }
}
