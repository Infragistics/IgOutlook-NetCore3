using IgOutlook.Core;
using IgOutlook.Core.Converters;
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
    /// Interaction logic for MeetingView.xaml
    /// </summary>
    [DependentView(typeof(MeetingHomeTab), RegionNames.RibbonTabRegion)]
    [DependentView(typeof(FormatTextTab), RegionNames.RibbonTabRegion)]
    public partial class MeetingView : ISupportDataContext, ISupportRichText
    {
        public XamRichTextEditor RichTextEditor { get; set; }

        public MeetingView()
        {
            InitializeComponent();

            //ComboEditors
            XamComboEditor cbo1 = new XamComboEditor();
            cbo1.Margin = new System.Windows.Thickness(2);
            cbo1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            cbo1.AllowMultipleSelection = true;
            cbo1.MultiSelectValueDelimiter = ';';
            cbo1.AutoComplete = true;
            cbo1.DisplayMemberPath = "Email";
            cbo1.SelectedValuePath = "Email";
            cbo1.Margin = new System.Windows.Thickness(2, 4, 2, 4);

            Binding itemsSourceBinding = new Binding();
            itemsSourceBinding.Source = DataContext;
            itemsSourceBinding.Path = new System.Windows.PropertyPath("Contacts");
            cbo1.SetBinding(XamComboEditor.ItemsSourceProperty, itemsSourceBinding);

            Binding selectedValuesBinding = new Binding();
            selectedValuesBinding.Source = DataContext;
            selectedValuesBinding.Path = new System.Windows.PropertyPath("Activity.Metadata[To]");
            selectedValuesBinding.Mode = BindingMode.TwoWay;
            selectedValuesBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            selectedValuesBinding.Converter = new ObservableCollectionToArrayConverter();
            cbo1.SetBinding(XamComboEditor.SelectedValuesProperty, selectedValuesBinding);

            Grid.SetColumnSpan(cbo1, 10);
            Grid.SetRow(cbo1, 0);
            Grid.SetColumn(cbo1, 2);

            _grid.Children.Add(cbo1);

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
            Grid.SetColumn(dti1, 2);

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
            Grid.SetColumn(dti2, 2);

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
