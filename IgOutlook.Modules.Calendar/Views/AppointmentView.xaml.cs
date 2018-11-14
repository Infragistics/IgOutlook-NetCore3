using IgOutlook.Core;
using IgOutlook.Modules.Calendar.TabItems;
using Infragistics.Controls.Editors;
using System;

namespace IgOutlook.Modules.Calendar.Views
{
    /// <summary>
    /// Interaction logic for AppointmentView.xaml
    /// </summary>
    [DependentView(typeof(AppointmentHomeTab), RegionNames.RibbonTabRegion)]
    public partial class AppointmentView : ISupportDataContext, ISupportRichText
    {
        public XamRichTextEditor RichTextEditor { get { return _richTextEditor; } set { throw new NotImplementedException(); } }

        public AppointmentView()
        {
            InitializeComponent();
        }
    }
}
