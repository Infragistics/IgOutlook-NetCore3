namespace IgOutlook.Core.Dialogs
{
    public interface IMessageBoxService
    {
        InteractionResult Show(string title, string message);
        InteractionResult Show(string title, string message, MessageBoxButtons buttons);
    }
}
