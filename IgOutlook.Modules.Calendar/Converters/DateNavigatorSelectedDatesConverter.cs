using Infragistics.Controls.Editors;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace IgOutlook.Modules.Calendar.Converters
{
    public class DateNavigatorSelectedDatesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var args = (SelectedDatesChangedEventArgs)value;
            return new ObservableCollection<DateTime>(args.AddedDates);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
