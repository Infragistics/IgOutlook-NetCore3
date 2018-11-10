using IgOutlook.Core;

namespace IgOutlook.Modules.Calendar.TabItems
{
    /// <summary>
    /// Interaction logic for AppointmentHomeTab.xaml
    /// </summary>
    public partial class AppointmentHomeTab : ISupportDataContext
    {
        public AppointmentHomeTab()
        {
            InitializeComponent();
            SetResourceReference(StyleProperty, typeof(Infragistics.Windows.Ribbon.RibbonTabItem));
        }
    }
}
