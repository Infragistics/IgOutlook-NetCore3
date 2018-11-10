using IgOutlook.Core;
using IgOutlook.Modules.Contacts.TabItems;
using Infragistics.Controls.Editors;
using Infragistics.Documents.RichText.Rtf;
using System.Windows.Data;

namespace IgOutlook.Modules.Contacts.Views
{
    /// <summary>
    /// Interaction logic for ContactDetailsView.xaml
    /// </summary>
    [DependentView(typeof(ContactHomeTab), RegionNames.RibbonTabRegion)]
    public partial class ContactDetailsView : ISupportDataContext
    {
        public ContactDetailsView()
        {
            InitializeComponent();

            //workaround for build error
            XamRichTextEditor rte = new XamRichTextEditor();
            rte.Name = "_rte";
            rte.AllowDocumentViewSplitting = false;
            rte.Margin = new System.Windows.Thickness(5);
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
            valueBinding.Path = new System.Windows.PropertyPath("ActiveContact.Notes");

            docAdapter.SetBinding(RtfDocumentAdapter.ValueProperty, valueBinding);
        }
    }
}
