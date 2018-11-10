using System;

namespace IgOutlook.Core.Dialogs
{
    public interface IDialogAware
    {
        bool CanCloseDialog();
        
        event Action RequestClose;
    }
}
