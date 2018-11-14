using IgOutlook.Core;
using IgOutlook.Modules.Calendar.TabItems;
using Infragistics.Controls.Editors;
using System;

namespace IgOutlook.Modules.Calendar.Views
{
    /// <summary>
    /// Interaction logic for MeetingView.xaml
    /// </summary>
    [DependentView(typeof(MeetingHomeTab), RegionNames.RibbonTabRegion)]
    [DependentView(typeof(FormatTextTab), RegionNames.RibbonTabRegion)]
    public partial class MeetingView : ISupportDataContext, ISupportRichText
    {
        public XamRichTextEditor RichTextEditor { get { return _richTextEditor; } set { throw new NotImplementedException(); } }

        public MeetingView()
        {
            InitializeComponent();
        }
    }
}
