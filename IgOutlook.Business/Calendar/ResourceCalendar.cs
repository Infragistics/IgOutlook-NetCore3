using IgOutlook.Business.Core;
using System.Windows.Media;

namespace IgOutlook.Business.Calendar
{

    public class ResourceCalendar : BusinessBase
    {
        private Color? _baseColor;
        public Color? BaseColor
        {
            get { return _baseColor; }
            set { SetProperty(ref _baseColor, value); }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private bool? _isVisible;
        public bool? IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _owningResourceId;
        public string OwningResourceId
        {
            get { return _owningResourceId; }
            set { SetProperty(ref _owningResourceId, value); }
        }

        public void SetCalendarVisibility(bool isVisible)
        {
            this._isVisible = isVisible;
        }
    }
}
