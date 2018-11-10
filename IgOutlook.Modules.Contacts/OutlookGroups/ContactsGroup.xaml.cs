using IgOutlook.Business;
using IgOutlook.Core;
using Infragistics.Controls.Menus;

namespace IgOutlook.Modules.Contacts.OutlookGroups
{
    /// <summary>
    /// Interaction logic for ContactsGroup.xaml
    /// </summary>
    public partial class ContactsGroup : IOutlookBarGroup
    {
        INavigationItem _selectedItem;

        public string DefaultNavigationPath
        {
            get { return "ContactsView"; }
        }

        private void ActiveNodeChanging(object sender, ActiveNodeChangingEventArgs e)
        {
            var _selectedItem = e.NewActiveTreeNode.Data as INavigationItem;
            if (_selectedItem != null && !_selectedItem.CanNavigate)
                e.Cancel = true;
        }

        public ContactsGroup()
        {
            InitializeComponent();
        }
    }
}
