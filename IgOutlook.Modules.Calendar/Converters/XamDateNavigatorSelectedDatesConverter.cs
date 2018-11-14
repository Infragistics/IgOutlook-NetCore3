using Infragistics.Controls.Editors;
using Infragistics.Controls.Schedules;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace IgOutlook.Modules.Calendar.Converters
{
    public class XamDateNavigatorSelectedDatesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateNav = parameter as XamDateNavigator;
            if (dateNav != null)
                return new ObservableCollection<DateTime>(dateNav.SelectedDates);

            else
            {
                var args = (SelectedDatesChangedEventArgs)value;
                return new ObservableCollection<DateTime>(args.AddedDates);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
