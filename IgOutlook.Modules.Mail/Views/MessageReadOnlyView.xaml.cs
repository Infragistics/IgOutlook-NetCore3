using IgOutlook.Core;
using IgOutlook.Modules.Mail.TabItems;
using Infragistics.Controls.Editors;
using Infragistics.Documents.RichText.Rtf;
using System.Windows.Controls;
using System.Windows.Data;

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

            //workaround for build error
            XamRichTextEditor rte = new XamRichTextEditor();
            rte.Name = "_rte";
            rte.AllowDocumentViewSplitting = false;
            rte.BorderThickness = new System.Windows.Thickness(0);
            rte.Margin = new System.Windows.Thickness(10);
            rte.CaretColor = null;
            rte.IsReadOnly = true;
            //rte.ClipboardSerializationProviders.Add(new RtfSerializationProvider());

            RtfDocumentAdapter docAdapter = new RtfDocumentAdapter();

            _rtePlaceholder.Children.Add(docAdapter);
            _rtePlaceholder.Children.Add(rte);

            Binding docBinding = new Binding();
            docBinding.Source = rte;
            docBinding.Path = new System.Windows.PropertyPath("Document");

            docAdapter.SetBinding(RtfDocumentAdapter.DocumentProperty, docBinding);

            Binding valueBinding = new Binding();
            valueBinding.Source = DataContext;
            valueBinding.Path = new System.Windows.PropertyPath("Message.Body");

            docAdapter.SetBinding(RtfDocumentAdapter.ValueProperty, valueBinding);
        }
    }
}
