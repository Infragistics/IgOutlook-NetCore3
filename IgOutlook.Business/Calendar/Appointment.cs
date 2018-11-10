using IgOutlook.Business.Core;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IgOutlook.Business.Calendar
{
    public class Appointment : BusinessBase, IEditableObject
    {        
        private bool _isNewVariance;
        public bool IsNewVariance
        {
            get { return _isNewVariance; }
            set { SetProperty(ref _isNewVariance, value); }
        }

        private bool _isLocation;
        public bool IsLocation
        {
            get { return _isLocation; }
            set { SetProperty(ref _isLocation, value); }
        }

        private ObservableCollection<string> _appointmentIds;
        public ObservableCollection<string> AppointmentIds
        {
            get { return _appointmentIds; }
            set { SetProperty(ref _appointmentIds, value); }
        }

        private ObservableCollection<string> _to;
        public ObservableCollection<string> To
        {
            get { return _to; }
            set { SetProperty(ref _to, value); }
        }

        private bool _isMeetingRequest;
        public bool IsMeetingRequest
        {
            get { return _isMeetingRequest; }
            set { SetProperty(ref _isMeetingRequest, value); }
        }

        private bool _reminderEnabled;
        public bool ReminderEnabled
        {
            get { return _reminderEnabled; }
            set { SetProperty(ref _reminderEnabled, value); }
        }

        private int _recurrenceVersion;
        public int RecurrenceVersion
        {
            get { return _recurrenceVersion; }
            set { SetProperty(ref _recurrenceVersion, value); }
        }

        private long _variantProperties;
        public long VariantProperties
        {
            get { return _variantProperties; }
            set { SetProperty(ref _variantProperties, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private bool? _isLocked;
        public bool? IsLocked
        {
            get { return _isLocked; }
            set { SetProperty(ref _isLocked, value); }
        }

        private bool _isOccurrenceDeleted;
        public bool IsOccurrenceDeleted
        {
            get { return _isOccurrenceDeleted; }
            set { SetProperty(ref _isOccurrenceDeleted, value); }
        }

        private bool _isTimeZoneNeutral;
        public bool IsTimeZoneNeutral
        {
            get { return _isTimeZoneNeutral; }
            set { SetProperty(ref _isTimeZoneNeutral, value); }
        }

        private bool? _isVisible;
        public bool? IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        private DateTime? _maxOccurrenceDateTime;
        public DateTime? MaxOccurrenceDateTime
        {
            get { return _maxOccurrenceDateTime; }
            set { SetProperty(ref _maxOccurrenceDateTime, value); }
        }

        private DateTime _originalOccurrenceEnd;
        public DateTime OriginalOccurrenceEnd
        {
            get { return _originalOccurrenceEnd; }
            set { SetProperty(ref _originalOccurrenceEnd, value); }
        }

        private DateTime _originalOccurrenceStart;
        public DateTime OriginalOccurrenceStart
        {
            get { return _originalOccurrenceStart; }
            set { SetProperty(ref _originalOccurrenceStart, value); }
        }

        private string _owningCalendarId;
        public string OwningCalendarId
        {
            get { return _owningCalendarId; }
            set { SetProperty(ref _owningCalendarId, value); }
        }

        private string _owningResourceId;
        public string OwningResourceId
        {
            get { return _owningResourceId; }
            set { SetProperty(ref _owningResourceId, value); }
        }

        private string _recurrence;
        public string Recurrence
        {
            get { return _recurrence; }
            set { SetProperty(ref _recurrence, value); }
        }

        private string _reminder;
        public string Reminder
        {
            get { return _reminder; }
            set { SetProperty(ref _reminder, value); }
        }

        private TimeSpan _reminderInterval;
        public TimeSpan ReminderInterval
        {
            get { return _reminderInterval; }
            set { SetProperty(ref _reminderInterval, value); }
        }

        private string _rootActivityId;
        public string RootActivityId
        {
            get { return _rootActivityId; }
            set { SetProperty(ref _rootActivityId, value); }
        }

        private DateTime _start;
        public DateTime Start
        {
            get { return _start; }
            set { SetProperty(ref _start, value); }
        }

        private string _startTimeZoneId;
        public string StartTimeZoneId
        {
            get { return _startTimeZoneId; }
            set { SetProperty(ref _startTimeZoneId, value); }
        }

        private string _subject;
        public string Subject
        {
            get { return _subject; }
            set { SetProperty(ref _subject, value); }
        }

        private string _unmappedProperties;
        public string UnmappedProperties
        {
            get { return _unmappedProperties; }
            set { SetProperty(ref _unmappedProperties, value); }
        }

        private DateTime _end;
        public DateTime End
        {
            get { return _end; }
            set { SetProperty(ref _end, value); }
        }

        private string _endTimeZoneId;
        public string EndTimeZoneId
        {
            get { return _endTimeZoneId; }
            set { SetProperty(ref _endTimeZoneId, value); }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }

        private string _categories;
        public string Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        #region IEditableObject

        private Appointment _originalObject;
        private bool _isInEdit;

        public void BeginEdit()
        {
            if (_isInEdit) return;

            _originalObject = (Appointment)this.MemberwiseClone();

            _isInEdit = true;
        }

        public void CancelEdit()
        {
            if (_isInEdit)
            {
                CopyProperties(_originalObject);

                _isInEdit = false;
            }
        }

        public void EndEdit()
        {
            if (!_isInEdit) return;

            _isInEdit = false;
            _originalObject = null;
        }

        public Appointment Clone()
        {
            var newobj = new Appointment();
            newobj.CopyProperties(this);
            return newobj;
        }

        #endregion //IEditableObject

    }
}
