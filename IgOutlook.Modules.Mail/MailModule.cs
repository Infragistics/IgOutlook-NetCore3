using IgOutlook.Core;
using IgOutlook.Modules.Mail.OutlookGroups;
using IgOutlook.Modules.Mail.Views;
using IgOutlook.Services;
using Prism.Ioc;
using Prism.Mvvm;
using System.Text;

namespace IgOutlook.Modules.Mail
{
    public class MailModule : ModuleBase
    {
        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<MailGroup, MailGroupViewModel>();

            containerRegistry.RegisterForNavigation<MessageListView>("MessageListView");
            containerRegistry.RegisterForNavigation<MessageView>("MessageView");
            containerRegistry.RegisterForNavigation<MessageReadOnlyView>("MessageReadOnlyView");

            containerRegistry.RegisterSingleton<IMailService, MailService>();
            containerRegistry.RegisterSingleton<ICategoryService, CategoryService>();
            containerRegistry.RegisterSingleton<IContactService, ContactService>();
        }

        protected override void ResolveOutlookGroup()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.OutlookBarGroupRegion, typeof(MailGroup));
        }
    }
}
