using IgOutlook.Business.Mail;
using Prism.Events;

namespace IgOutlook.Modules.Mail.Events
{
    public class MessageDeletedEvent : PubSubEvent<MailMessage> { }
}
