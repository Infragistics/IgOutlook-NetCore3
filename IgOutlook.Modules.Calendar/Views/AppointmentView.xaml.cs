using IgOutlook.Core;
using IgOutlook.Modules.Calendar.Converters;
using IgOutlook.Modules.Calendar.TabItems;
using Infragistics.Controls.Editors;
using Infragistics.Documents.RichText.Rtf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace IgOutlook.Modules.Calendar.Views
{
    /// <summary>
    /// Interaction logic for AppointmentView.xaml
    /// </summary>
    [DependentView(typeof(AppointmentHomeTab), RegionNames.RibbonTabRegion)]
    public partial class AppointmentView : ISupportDataContext, ISupportRichText
    {
        public XamRichTextEditor RichTextEditor { get; set; }

        public AppointmentView()
        {
            InitializeComponent();

            //date/time inputs
            XamDateTimeInput dti1 = new XamDateTimeInput();
            dti1.Margin = new System.Windows.Thickness(2, 4, 2, 4);
            dti1.DropDownButtonStyle = (Style)Resources["DropDownButtonStyle"];
            dti1.DropDownButtonDisplayMode = DropDownButtonDisplayMode.Always;

            Binding dtiValueBinding = new Binding();
            dtiValueBinding.Source = DataContext;
            dtiValueBinding.Path = new System.Windows.PropertyPath("Activity.Start");
            dtiValueBinding.Converter = new ActivityUtcToLocalTimeConverter();
            dti1.SetBinding(XamDateTimeInput.ValueProperty, dtiValueBinding);

            Grid.SetRow(dti1, 3);
            Grid.SetColumn(dti1, 1);

            _grid.Children.Add(dti1);

            XamDateTimeInput dti2 = new XamDateTimeInput();
            dti2.Margin = new System.Windows.Thickness(2, 4, 2, 4);
            dti2.DropDownButtonStyle = (Style)Resources["DropDownButtonStyle"];
            dti2.DropDownButtonDisplayMode = DropDownButtonDisplayMode.Always;

            Binding dtiValueBinding2 = new Binding();
            dtiValueBinding2.Source = DataContext;
            dtiValueBinding2.Path = new System.Windows.PropertyPath("Activity.End");
            dtiValueBinding2.Converter = new ActivityUtcToLocalTimeConverter();
            dti2.SetBinding(XamDateTimeInput.ValueProperty, dtiValueBinding2);

            Grid.SetRow(dti2, 4);
            Grid.SetColumn(dti2, 1);

            _grid.Children.Add(dti2);

            //RichTextEditor
            RichTextEditor = new XamRichTextEditor();
            RichTextEditor.Name = "_rte";
            RichTextEditor.AllowDocumentViewSplitting = false;
            //rte.ClipboardSerializationProviders.Add(new RtfSerializationProvider());

            RtfDocumentAdapter docAdapter = new RtfDocumentAdapter();
            docAdapter.RefreshTrigger = Infragistics.Documents.RichText.Serialization.RichTextRefreshTrigger.ContentChanged;

            Binding docBinding = new Binding();
            docBinding.Source = RichTextEditor;
            docBinding.Path = new System.Windows.PropertyPath("Document");

            docAdapter.SetBinding(RtfDocumentAdapter.DocumentProperty, docBinding);

            Binding valueBinding = new Binding();
            valueBinding.Source = DataContext;
            valueBinding.Path = new System.Windows.PropertyPath("Activity.Description");

            docAdapter.SetBinding(RtfDocumentAdapter.ValueProperty, valueBinding);

            _rtePlaceholder.Children.Add(docAdapter);
            _rtePlaceholder.Children.Add(RichTextEditor);
        }
    }
}
