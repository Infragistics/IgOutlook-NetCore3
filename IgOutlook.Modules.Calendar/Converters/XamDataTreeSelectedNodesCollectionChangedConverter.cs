using Infragistics.Controls.Menus;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace IgOutlook.Modules.Calendar.Converters
{
    public class XamDataTreeSelectedNodesCollectionChangedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var args = (NodeSelectionEventArgs)value;
            if (args.CurrentSelectedNodes.Count < args.OriginalSelectedNodes.Count)
            {
                var removed = args.OriginalSelectedNodes.Select(n => n.Data).Except(args.CurrentSelectedNodes.Select(n => n.Data));

                if (removed.Count() > 0)
                {
                    return removed.First();
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
