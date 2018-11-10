using IgOutlook.Business.Core;
using System;

namespace IgOutlook.Business.Calendar
{
    public class Resource : BusinessBase
    {
        private string _daySettingsOverrides;
        public string DaySettingsOverrides
        {
            get { return _daySettingsOverrides; }
            set { SetProperty(ref _daySettingsOverrides, value); }
        }

        private string _daysOfWeek;
        public string DaysOfWeek
        {
            get { return _daysOfWeek; }
            set { SetProperty(ref _daysOfWeek, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _emailAddress;
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { SetProperty(ref _emailAddress, value); }
        }

        private DayOfWeek? _firstDayOfWeek;
        public DayOfWeek? FirstDayOfWeek
        {
            get { return _firstDayOfWeek; }
            set { SetProperty(ref _firstDayOfWeek, value); }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private bool _isLocked;
        public bool IsLocked
        {
            get { return _isLocked; }
            set { SetProperty(ref _isLocked, value); }
        }

        private bool? _isVisible;
        public bool? IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _primaryCalendarId;
        public string PrimaryCalendarId
        {
            get { return _primaryCalendarId; }
            set { SetProperty(ref _primaryCalendarId, value); }
        }

        private string _primaryTimeZoneId;
        public string PrimaryTimeZoneId
        {
            get { return _primaryTimeZoneId; }
            set { SetProperty(ref _primaryTimeZoneId, value); }
        }

        private string _unmappedProperties;
        public string UnmappedProperties
        {
            get { return _unmappedProperties; }
            set { SetProperty(ref _unmappedProperties, value); }
        }

    }
}
