using IgOutlook.Business.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace IgOutlook.Modules.Mail.ViewModels
{
    public class FieldSortOption : BusinessBase
    {
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value); }
        }

        private string _label;
        public string Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }

        private string _fieldName;
        public string FieldName
        {
            get { return _fieldName; }
            set { SetProperty(ref _fieldName, value); }
        }

        private ListSortDirection _activeSortDirection;
        public ListSortDirection ActiveSortDirection
        {
            get { return _activeSortDirection; }
            set
            {
                SetProperty(ref _activeSortDirection, value);
                RaisePropertyChanged("ActiveSortDescription");
            }
        }

        public string ActiveSortDescription
        {
            get { return SortOptions[_activeSortDirection]; }
        }

        public Dictionary<ListSortDirection, string> SortOptions { get; set; }

        public FieldSortOption(string fieldName, string label, string ascendingOption, string descendingOption)
        {
            FieldName = fieldName;
            Label = label;
            SortOptions = new Dictionary<ListSortDirection, string>();
            SortOptions.Add(ListSortDirection.Ascending, ascendingOption);
            SortOptions.Add(ListSortDirection.Descending, descendingOption);
            ActiveSortDirection = ListSortDirection.Descending;
        }

        public FieldSortOption(string fieldName, string ascendingOption, string descendingOption)
        {
            FieldName = fieldName;
            Label = fieldName;
            SortOptions = new Dictionary<ListSortDirection, string>();
            SortOptions.Add(ListSortDirection.Ascending, ascendingOption);
            SortOptions.Add(ListSortDirection.Descending, descendingOption);
            ActiveSortDirection = ListSortDirection.Descending;
        }
    }
}
