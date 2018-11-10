using IgOutlook.Core;
using IgOutlook.Modules.Contacts.TabItems;

namespace IgOutlook.Modules.Contacts.Views
{
    /// <summary>
    /// Interaction logic for ContactsView.xaml
    /// </summary>
    [DependentView(typeof(HomeTab), RegionNames.RibbonTabRegion)]
    public partial class ContactsView : ISupportDataContext
    {
        public ContactsView()
        {
            InitializeComponent();
        }
    }
}
