using IgOutlook.Core;
using IgOutlook.Modules.Mail.TabItems;
using System;

namespace IgOutlook.Modules.Mail.Views
{
    /// <summary>
    /// Interaction logic for MessageView.xaml
    /// </summary>
    [DependentView(typeof(MessageHomeTab), RegionNames.RibbonTabRegion)]
    public partial class MessageView : ISupportDataContext, ISupportRichText
    {
        public Infragistics.Controls.Editors.XamRichTextEditor RichTextEditor { get { return _richTextEditor; } set { throw new NotImplementedException(); } }

        public MessageView()
        {
            InitializeComponent();
        }
    }
}
