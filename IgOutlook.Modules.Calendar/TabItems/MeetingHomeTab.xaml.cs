using IgOutlook.Core;
using Infragistics.Controls.Editors;

namespace IgOutlook.Modules.Calendar.TabItems
{
    /// <summary>
    /// Interaction logic for MeetingHomeTab.xaml
    /// </summary>
    public partial class MeetingHomeTab : ISupportDataContext
    {
        public MeetingHomeTab()
        {
            InitializeComponent();

            SetResourceReference(StyleProperty, typeof(Infragistics.Windows.Ribbon.RibbonTabItem));
        }
    }
}
