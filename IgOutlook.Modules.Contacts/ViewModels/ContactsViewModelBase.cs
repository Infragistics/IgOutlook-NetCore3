using IgOutlook.Business.Contacts;
using IgOutlook.Core;
using IgOutlook.Core.Dialogs;
using IgOutlook.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace IgOutlook.Modules.Contacts.ViewModels
{
    public class ContactsViewModelBase : NavigationAwareViewModelBase
    {
        #region Properties

        protected IEventAggregator EventAggregator { get; private set; }
        protected IContactService ContactService { get; private set; }
        protected IDialogService DialogService { get; private set; }

        public DelegateCommand<string> ContactCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }

        private Contact _activeContact;
        public Contact ActiveContact
        {
            get { return _activeContact; }
            set { SetProperty(ref _activeContact, value); }
        }

        #endregion //Properties

        #region Constructors

        public ContactsViewModelBase(IEventAggregator eventAggragator, IContactService contactService, IDialogService dialogService)
        {
            EventAggregator = eventAggragator;
            ContactService = contactService;
            DialogService = dialogService;

            ContactCommand = new DelegateCommand<string>(ExecuteConactCommand, CanExecuteConactCommand);
            DeleteCommand = new DelegateCommand(DeleteContact, HasValidContact);

            this.PropertyChanged += (s, a) =>
            {
                if (a.PropertyName == "ActiveContact")
                    DeleteCommand.RaiseCanExecuteChanged();
            };

            IconSource = "pack://application:,,,/IgOutlook.Core;component/Images/Contact.ico";
        }

        #endregion //Constructors

        #region Commands

        protected virtual void ExecuteConactCommand(string action)
        {
            string view = "ContactDetailsView";
            NavigationParameters parameters = null;

            if (action.Equals(ContactParameters.New))
            {
                parameters = CreateContactParameters(ContactParameters.New);
            }
            else if (action.Equals(ContactParameters.View))
            {
                if (ActiveContact == null) return;
                view = "ContactDetailsView";
                parameters = CreateContactParameters(ContactParameters.View);
            }

            DialogService.ShowDialog(view, parameters);
        }

        protected virtual bool CanExecuteConactCommand(string action)
        {
            return true;
        }

        protected virtual void DeleteContact()
        {
            ContactService.DeleteContact(ActiveContact);
        }

        protected virtual bool HasValidContact()
        {
            return ActiveContact != null;
        }

        #endregion //Commands

        #region Methods

        protected virtual NavigationParameters CreateContactParameters(string contactMode)
        {
            NavigationParameters parameters = new NavigationParameters();

            parameters.Add(ContactParameters.ContactModeKey, contactMode);

            var sourceContactId = "NEW";
            if (!contactMode.Equals(ContactParameters.New))
            {
                sourceContactId = _activeContact.Id.ToString();
            }
            parameters.Add(ContactParameters.ContactIdKey, sourceContactId.ToString());

            return parameters;
        }

        #endregion //Methods
    }
}
