using IgOutlook.Core;
using IgOutlook.Modules.Mail.TabItems;

namespace IgOutlook.Modules.Mail.Views
{
    /// <summary>
    /// Interaction logic for MessageReadOnlyView.xaml
    /// </summary>
    [DependentView(typeof(MessageReadOnlyHomeTab), RegionNames.RibbonTabRegion)]
    public partial class MessageReadOnlyView : ISupportDataContext
    {
        public MessageReadOnlyView()
        {
            InitializeComponent();
        }
    }
}
