using IgOutlook.Core;

namespace IgOutlook.Modules.Mail.TabItems
{
    /// <summary>
    /// Interaction logic for MessageReadOnlyHomeTab.xaml
    /// </summary>
    public partial class MessageReadOnlyHomeTab : ISupportDataContext
    {
        public MessageReadOnlyHomeTab()
        {
            InitializeComponent();
            SetResourceReference(StyleProperty, typeof(Infragistics.Windows.Ribbon.RibbonTabItem));
        }
    }
}
