using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace IgOutlook.Core.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogShell.xaml
    /// </summary>
    public partial class DialogShell
    {
        IDialogAware _viewModel;

        public DialogShell()
        {
            InitializeComponent();
            Closed += RibbonDialog_Closed;
            Closing += RibbonDialog_Closing;
        }

        public void Show(bool isModal)
        {
            base.Show();

            var fe = _contentRegion.Content as FrameworkElement;
            if (fe != null)
            {
                _viewModel = fe.DataContext as IDialogAware;

                if (_viewModel != null)
                {
                    _viewModel.RequestClose += ViewModel_RequestClose;

                    var viewModelBase = (ViewModelBase)fe.DataContext;

                    if (!string.IsNullOrEmpty(viewModelBase.IconSource))
                    {
                        Uri iconUri = new Uri(viewModelBase.IconSource, UriKind.RelativeOrAbsolute);
                        this.Icon = BitmapFrame.Create(iconUri);
                    }

                    viewModelBase.PropertyChanged += (s, a) =>
                    {
                        if (a.PropertyName == "Title")
                            if (viewModelBase.Title != null)
                                this.Title = viewModelBase.Title;
                    };

                    viewModelBase.RefreshTitle();
                }
            }

            CalculateDialogWindowsPosition();
        }

        void ViewModel_RequestClose()
        {
            this.Close();
        }

        void RibbonDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_viewModel != null && !_viewModel.CanCloseDialog())
                e.Cancel = true;
        }

        void RibbonDialog_Closed(object sender, EventArgs e)
        {
            if (_viewModel != null)
                _viewModel.RequestClose -= ViewModel_RequestClose;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CalculateDialogWindowsPosition()
        {
            var windows = Application.Current.Windows;

            int x = 0;
            foreach (var window in windows)
            {
                if (window is IDialog)
                {
                    if (window == this)
                    {
                        if (x > 1) //don't want the Shell
                        {
                            for (int i = x - 1; i > 0; i--)
                            {
                                if (windows[i] is IDialog)
                                {
                                    var priorWindow = windows[i];
                                    Left = priorWindow.Left - 40;
                                    Top = priorWindow.Top + 40;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
                x++;
            }
        }
    }
}
