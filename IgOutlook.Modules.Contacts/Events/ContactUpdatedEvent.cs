using IgOutlook.Business.Contacts;
using Prism.Events;

namespace IgOutlook.Modules.Contacts.Events
{
    public class ContactUpdatedEvent : PubSubEvent<Contact> { }
}
