using System;
using IgOutlook.Core.Dialogs;
using IgOutlook.Modules.Mail.Events;
using IgOutlook.Modules.Mail.Resources;
using IgOutlook.Services;
using Prism.Events;
using Prism.Regions;

namespace IgOutlook.Modules.Mail.ViewModels
{
    public class MessageReadOnlyViewModel : MessageViewModelBase, IDialogAware
    {
        string _messageId;

        public MessageReadOnlyViewModel(IEventAggregator eventAggragator, IMailService mailService, IDialogService dialogService, ICategoryService categoryService, IContactService contactService) 
            : base(eventAggragator, mailService, dialogService, categoryService, contactService)
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            _messageId = navigationContext.Parameters[MessageParameters.MessageId] as string;

            LoadMessage(_messageId);
        }

        protected override void DeleteMessage()
        {
            base.DeleteMessage();
            RequestClose();
            EventAggregator.GetEvent<MessageDeletedEvent>().Publish(Message);
        }

        async void LoadMessage(string messageId)
        {
            Message = await MailService.GetMessageByIdAsync(messageId);

            Title = Message.Subject + " - " + ResourceStrings.Message_Text;
        }

        #region IDialogAware Members

        public bool CanCloseDialog()
        {
            return true;
        }

        public event Action RequestClose;

        void CloseDialog()
        {
            RequestClose?.Invoke();
        }

        #endregion //IDialogAware Members
    }
}
