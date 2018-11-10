using IgOutlook.Business.Contacts;
using IgOutlook.Core.Dialogs;
using IgOutlook.Core.Events;
using IgOutlook.Modules.Contacts.Events;
using IgOutlook.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IgOutlook.Modules.Contacts.ViewModels
{
    public class ContactsViewModel : ContactsViewModelBase
    {
        #region Constructor

        public ContactsViewModel(IEventAggregator eventAggragator, IContactService contactService, IDialogService dialogService)
            : base(eventAggragator, contactService, dialogService)
        {
            ActivateContactCommand = new DelegateCommand<string>(ActivateContact);

            Contacts = contactService.GetContacts();

            EventAggregator.GetEvent<ContactUpdatedEvent>().Subscribe((contact) =>
            {
                if (ActiveContact != null)
                {
                    ActiveContact.CopyProperties(contact);
                }
            });

            EventAggregator.GetEvent<ContactDeletedEvent>().Subscribe((contact) =>
            {
                if (ActiveContact != null)
                {
                    Contacts.Remove(Contacts.First(c => c.Id == contact.Id));
                }
            });

            var viewItemsCountChangedEvent = EventAggregator.GetEvent<ViewItemsCountChangedEvent>();

            Contacts.CollectionChanged += (s, a) => viewItemsCountChangedEvent.Publish(Contacts.Count);
        }

        #endregion //Constructor

        #region Base Class Overrides

        protected override void DeleteContact()
        {
            base.DeleteContact();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            Title = Resources.ResourceStrings.OutlookBarGroup_Header_Contacts;
            EventAggregator.GetEvent<ViewActivateEvent>().Publish(Title);

            var viewItemsCountChangedEvent = EventAggregator.GetEvent<ViewItemsCountChangedEvent>();

            viewItemsCountChangedEvent.Publish(Contacts.Count);
        }

        #endregion //Base Class Overrides

        #region Public Members

        public ObservableCollection<Contact> Contacts { get; private set; }

        public List<string> AlphabeticalSearchItems { get { return Resources.ResourceStrings.Search_Alphabet_List.Split(',').ToList(); } }

        public DelegateCommand<string> ActivateContactCommand { get; private set; }

        #endregion //Public Members

        #region Commands

        private void ActivateContact(string findCreteria)
        {
            foreach (Contact contact in Contacts)
            {
                if (contact.LastName.ToLower().StartsWith(findCreteria))
                {
                    ActiveContact = contact;
                    return;
                }
            }
        }

        #endregion //Commands
    }
}
