using System;
using System.Globalization;
using System.Windows.Data;

namespace IgOutlook.Modules.Calendar.Converters
{
    public class XamOutlookCalendarViewSelectedTimeRangeChangedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var args = (Infragistics.Controls.NullableRoutedPropertyChangedEventArgs<Infragistics.DateRange>)value;
            return args.NewValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
