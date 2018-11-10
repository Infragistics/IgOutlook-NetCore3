using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using IgOutlook.Business.Mail;
using IgOutlook.Core.Dialogs;
using IgOutlook.Core.Events;
using IgOutlook.Modules.Mail.Events;
using IgOutlook.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace IgOutlook.Modules.Mail.ViewModels
{
    public class MessageListViewModel : MessageViewModelBase
    {
        private Dictionary<string, Business.Contacts.Contact> contacts;
        private ViewItemsCountChangedEvent _viewItemsCountChangedEvent;

        private string _searchMailText;
        public string SearchMailText
        {
            get { return _searchMailText; }
            set { SetProperty(ref _searchMailText, value); }
        }

        private ObservableCollection<MailMessage> _messages;
        public ObservableCollection<MailMessage> Messages
        {
            get { return _messages; }
            set
            {
                SetProperty(ref _messages, value);
                if (value != null)
                {
                    LinkEmailContacts();
                    _viewItemsCountChangedEvent.Publish(Messages.Count);

                    if (Message == null && value.Count > 0)
                        Message = value[0];
                }
            }
        }

        private ObservableCollection<FieldSortOption> _fieldSortOptions;
        public ObservableCollection<FieldSortOption> FieldSortOptions
        {
            get { return _fieldSortOptions; }
            set { SetProperty(ref _fieldSortOptions, value); }
        }

        private FieldSortOption _activeFieldSortOption;
        public FieldSortOption ActiveFieldSortOption
        {
            get { return _activeFieldSortOption; }
            set { SetProperty(ref _activeFieldSortOption, value); }
        }

        private string _searchMailNullText;

        public string SearchMailNullText
        {
            get { return _searchMailNullText; }
            set { SetProperty(ref _searchMailNullText, value); }
        }

        public DelegateCommand ToggleMailSortingCommand { get; private set; }
        public DelegateCommand<string> GroupByFieldCommand { get; private set; }
        public DelegateCommand ClearSearchMailTextCommand { get; private set; }

        public MessageListViewModel(IEventAggregator eventAggregator, IMailService mailService, IDialogService dialogService, ICategoryService categoryService, IContactService contactService) 
            : base(eventAggregator, mailService, dialogService, categoryService, contactService)
        {
            contacts = contactService.GetContactsAsDictionary();

            eventAggregator.GetEvent<MessageSentEvent>().Subscribe(MessageSent);
            eventAggregator.GetEvent<MessageDeletedEvent>().Subscribe(MessageDeleted);


            ToggleMailSortingCommand = new DelegateCommand(ToggleMailSorting);
            GroupByFieldCommand = new DelegateCommand<string>(GroupByField);
            ClearSearchMailTextCommand = new DelegateCommand(ClearSearchMailText, () => { return !string.IsNullOrEmpty(SearchMailText); }).ObservesProperty(() => SearchMailText);

            InitFieldSortOptions();

            SetActiveFieldSortOption(FieldSortOptions[0].FieldName);

            _viewItemsCountChangedEvent = EventAggregator.GetEvent<ViewItemsCountChangedEvent>();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            LoadMailbox(CurrentMailbox);

            Title = String.Format("{0}", FolderParameters.LocalizedFolderNames[CurrentMailbox]);
            EventAggregator.GetEvent<ViewActivateEvent>().Publish(Title);

            SearchMailNullText = Resources.ResourceStrings.Search_Text + " " + FolderParameters.LocalizedFolderNames[CurrentMailbox];
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            Messages = null;
            return base.IsNavigationTarget(navigationContext);
        }

        protected override void DeleteMessage()
        {
            base.DeleteMessage();
            MessageDeleted(Message);
        }

        protected override void OnMessageChanged()
        {
            base.OnMessageChanged();
            LoadMessageBody();
        }


        private void LoadMailbox(string mailbox)
        {
            if (mailbox == FolderParameters.Inbox)
                LoadInbox();
            if (mailbox == FolderParameters.Sent)
                LoadSentItems();
            if (mailbox == FolderParameters.Drafts)
                LoadDraftItems();
            if (mailbox == FolderParameters.Deleted)
                LoadDeletedItems();
        }

        async void LoadInbox()
        {
            Messages = new ObservableCollection<MailMessage>(await MailService.GetInboxItemsAsync());
        }

        async void LoadSentItems()
        {
            Messages = new ObservableCollection<MailMessage>(await MailService.GetSentItemsAsync());
        }

        async void LoadDraftItems()
        {
            Messages = new ObservableCollection<MailMessage>(await MailService.GetDraftItemsAsync());
        }

        async void LoadDeletedItems()
        {
            Messages = new ObservableCollection<MailMessage>(await MailService.GetDeletedItemsAsync());
        }

        void LoadMessageBody()
        {
            if (Message != null)
            {
                Message.MarkAsRead();
            }
        }

        public void MessageSent(MailMessage message)
        {
            if (CurrentMailbox == FolderParameters.Sent)
            {
                Messages.Add(message);
                _viewItemsCountChangedEvent.Publish(Messages.Count);
                LinkEmailContacts();
            }
        }

        public void MessageDeleted(MailMessage message)
        {
            if (Messages.Contains(message))
            {
                Messages.Remove(message);
                _viewItemsCountChangedEvent.Publish(Messages.Count);
            }
        }

        private void ToggleMailSorting()
        {
            ActiveFieldSortOption.ActiveSortDirection = ActiveFieldSortOption.ActiveSortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
        }

        private void InitFieldSortOptions()
        {
            FieldSortOptions = new ObservableCollection<FieldSortOption>();
            FieldSortOptions.Add(new FieldSortOption("DateSent", Resources.ResourceStrings.Date_Text, Resources.ResourceStrings.Oldest_Text, Resources.ResourceStrings.Newest_Text));
            FieldSortOptions.Add(new FieldSortOption("From", Resources.ResourceStrings.From_Text, Resources.ResourceStrings.AtoZ_Text, Resources.ResourceStrings.ZtoA_Text));
            FieldSortOptions.Add(new FieldSortOption("Subject", Resources.ResourceStrings.Subject_Text, Resources.ResourceStrings.AtoZ_Text, Resources.ResourceStrings.ZtoA_Text));
            FieldSortOptions.Add(new FieldSortOption("Importance", Resources.ResourceStrings.Importance_Text, Resources.ResourceStrings.High_Text, Resources.ResourceStrings.Low_Text));
        }

        private void GroupByField(string fieldName)
        {
            SetActiveFieldSortOption(fieldName);
        }

        private void SetActiveFieldSortOption(string fieldName)
        {
            ActiveFieldSortOption = FieldSortOptions.First(f => f.FieldName == fieldName);

            foreach (var fieldSortOption in FieldSortOptions)
            {
                fieldSortOption.IsActive = false;
            }
            ActiveFieldSortOption.IsActive = true;
        }

        private void LinkEmailContacts()
        {
            foreach (var message in Messages)
            {
                if (contacts.ContainsKey(message.From))
                    message.Contact = contacts[message.From];
            }
        }

        private void ClearSearchMailText()
        {
            SearchMailText = string.Empty;
        }
    }
}
