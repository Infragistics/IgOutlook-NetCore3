using IgOutlook.Business.Core;
using System.Windows.Media;

namespace IgOutlook.Business.Calendar
{
    public class ActivityCategory : BusinessBase
    {
        private Color _color;
        private object _dataItem;
        private string _description;
        private string _categoryName;

        public Color Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }
        public object DataItem
        {
            get { return _dataItem; }
            set { SetProperty(ref _dataItem, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        public string CategoryName
        {
            get { return _categoryName; }
            set { SetProperty(ref _categoryName, value); }
        }
    }
}
