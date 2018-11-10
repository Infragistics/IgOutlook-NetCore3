using IgOutlook.Business.Contacts;
using Prism.Events;

namespace IgOutlook.Modules.Contacts.Events
{
    public class ContactDeletedEvent : PubSubEvent<Contact> { }
}
