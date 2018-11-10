using IgOutlook.Core;
using IgOutlook.Modules.Contacts.OutlookGroups;
using IgOutlook.Modules.Contacts.Views;
using IgOutlook.Services;
using Prism.Ioc;
using Prism.Mvvm;

namespace IgOutlook.Modules.Contacts
{
    public class ContactsModule : ModuleBase
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<ContactsGroup, ContactsGroupViewModel>();

            containerRegistry.RegisterForNavigation<ContactsView>("ContactsView");
            containerRegistry.RegisterForNavigation<ContactDetailsView>("ContactDetailsView");

            containerRegistry.Register<IContactService, ContactService>();
        }

        protected override void ResolveOutlookGroup()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.OutlookBarGroupRegion, typeof(ContactsGroup));
        }
    }
}
