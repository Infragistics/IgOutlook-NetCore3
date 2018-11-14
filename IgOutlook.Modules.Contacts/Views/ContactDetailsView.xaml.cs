using IgOutlook.Core;
using IgOutlook.Modules.Contacts.TabItems;
using Infragistics.Controls.Editors;
using Infragistics.Documents.RichText.Rtf;
using System.Windows.Data;

namespace IgOutlook.Modules.Contacts.Views
{
    /// <summary>
    /// Interaction logic for ContactDetailsView.xaml
    /// </summary>
    [DependentView(typeof(ContactHomeTab), RegionNames.RibbonTabRegion)]
    public partial class ContactDetailsView : ISupportDataContext
    {
        public ContactDetailsView()
        {
            InitializeComponent();
        }
    }
}
