using IgOutlook.Core;

namespace IgOutlook.Modules.Contacts.TabItems
{
    /// <summary>
    /// Interaction logic for ContactHomeTab.xaml
    /// </summary>
    public partial class ContactHomeTab : ISupportDataContext
    {
        public ContactHomeTab()
        {
            InitializeComponent();
            SetResourceReference(StyleProperty, typeof(Infragistics.Windows.Ribbon.RibbonTabItem));
        }
    }
}
