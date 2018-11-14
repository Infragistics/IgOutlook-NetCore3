using IgOutlook.Core;
using IgOutlook.Modules.Mail.TabItems;
using System.Windows.Controls;

namespace IgOutlook.Modules.Mail.Views
{
    /// <summary>
    /// Interaction logic for MessageListView.xaml
    /// </summary>
    [DependentView(typeof(HomeTab), RegionNames.RibbonTabRegion)]
    public partial class MessageListView : UserControl, ISupportDataContext
    {
        public MessageListView()
        {
            InitializeComponent();
        }
    }
}
