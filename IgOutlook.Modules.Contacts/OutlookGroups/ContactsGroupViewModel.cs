using IgOutlook.Business;
using IgOutlook.Core;
using IgOutlook.Modules.Contacts.Resources;
using System.Collections.ObjectModel;

namespace IgOutlook.Modules.Contacts.OutlookGroups
{
    public class ContactsGroupViewModel : ViewModelBase
    {
        private ObservableCollection<NavigationItem> _items;
        public ObservableCollection<NavigationItem> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public ContactsGroupViewModel()
        {
            GenerateMenu();
        }

        private void GenerateMenu()
        {
            Items = new ObservableCollection<NavigationItem>();

            var root = new NavigationItem() { Caption = ResourceStrings.ContactsGroup_MyContacts_Text, NavigationPath = "ContactsView", CanNavigate = false };
            root.Items.Add(new NavigationItem() { Caption = ResourceStrings.ContactsGroup_Contacts_Text, NavigationPath = "ContactsView" });

            Items.Add(root);
        }

    }
}
