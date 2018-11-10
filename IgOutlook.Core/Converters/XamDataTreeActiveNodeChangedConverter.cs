using IgOutlook.Business;
using Infragistics.Controls.Menus;
using System;
using System.Globalization;
using System.Windows.Data;

namespace IgOutlook.Core.Converters
{
    public class XamDataTreeActiveNodeChangedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as ActiveNodeChangedEventArgs;

            if (eventArgs == null)
                return value;

            var navItem = eventArgs.NewActiveTreeNode?.Data as INavigationItem;
            if (navItem != null)
                return navItem.NavigationPath;
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
