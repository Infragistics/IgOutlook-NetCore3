using IgOutlook.Core;
using IgOutlook.ViewModels;
using Infragistics.Windows.OutlookBar;

namespace IgOutlook.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void XamOutlookBar_SelectedGroupChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            var group = ((XamOutlookBar)sender).SelectedGroup as IOutlookBarGroup;
            if (group != null)
            {
                ((MainWindowViewModel)DataContext).NavigateCommand.Execute(group.DefaultNavigationPath);
            }
        }
    }
}
