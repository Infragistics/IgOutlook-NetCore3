using Prism.Regions;
using System.Windows;

namespace IgOutlook.Core.Dialogs
{
    public interface IDialogService
    {
        FrameworkElement GetDialogInstance();

        void ShowDialog(string navigationUri, NavigationParameters parameters = null, bool isModal = true);
    }
}
