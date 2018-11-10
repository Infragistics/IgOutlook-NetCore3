using IgOutlook.Core;
using IgOutlook.Core.Converters;
using IgOutlook.Modules.Mail.TabItems;
using Infragistics.Controls.Editors;
using Infragistics.Documents.RichText.Rtf;
using System.Windows.Controls;
using System.Windows.Data;

namespace IgOutlook.Modules.Mail.Views
{
    /// <summary>
    /// Interaction logic for MessageView.xaml
    /// </summary>
    [DependentView(typeof(MessageHomeTab), RegionNames.RibbonTabRegion)]
    public partial class MessageView : ISupportDataContext, ISupportRichText
    {
        public Infragistics.Controls.Editors.XamRichTextEditor RichTextEditor { get; set; }

        public MessageView()
        {
            InitializeComponent();

            //workaround for build errors

            //ComboEditors
            XamComboEditor cbo1 = new XamComboEditor();
            cbo1.Margin = new System.Windows.Thickness(2);
            cbo1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            cbo1.AllowMultipleSelection = true;
            cbo1.MultiSelectValueDelimiter = ';';
            cbo1.AutoComplete = true;
            cbo1.DisplayMemberPath = "Email";
            cbo1.SelectedValuePath = "Email";

            Binding itemsSourceBinding = new Binding();
            itemsSourceBinding.Source = DataContext;
            itemsSourceBinding.Path = new System.Windows.PropertyPath("Contacts");
            cbo1.SetBinding(XamComboEditor.ItemsSourceProperty, itemsSourceBinding);

            Binding selectedValuesBinding = new Binding();
            selectedValuesBinding.Source = DataContext;
            selectedValuesBinding.Path = new System.Windows.PropertyPath("Message.To");
            selectedValuesBinding.Mode = BindingMode.TwoWay;
            selectedValuesBinding.Converter = new ObservableCollectionToArrayConverter();
            cbo1.SetBinding(XamComboEditor.SelectedValuesProperty, selectedValuesBinding);

            Grid.SetRow(cbo1, 1);
            Grid.SetColumn(cbo1, 2);

            XamComboEditor cbo2 = new XamComboEditor();
            cbo2.Margin = new System.Windows.Thickness(2);
            cbo2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            cbo2.AllowMultipleSelection = true;
            cbo2.MultiSelectValueDelimiter = ';';
            cbo2.AutoComplete = true;
            cbo2.DisplayMemberPath = "Email";
            cbo2.SelectedValuePath = "Email";

            Binding itemsSourceBinding2 = new Binding();
            itemsSourceBinding2.Source = DataContext;
            itemsSourceBinding2.Path = new System.Windows.PropertyPath("Contacts");
            cbo2.SetBinding(XamComboEditor.ItemsSourceProperty, itemsSourceBinding2);

            Binding selectedValuesBindin2g = new Binding();
            selectedValuesBindin2g.Source = DataContext;
            selectedValuesBindin2g.Path = new System.Windows.PropertyPath("Message.Cc");
            selectedValuesBindin2g.Mode = BindingMode.TwoWay;
            selectedValuesBindin2g.Converter = new ObservableCollectionToArrayConverter();
            cbo2.SetBinding(XamComboEditor.SelectedValuesProperty, selectedValuesBindin2g);

            Grid.SetRow(cbo2, 2);
            Grid.SetColumn(cbo2, 2);

            _grid.Children.Add(cbo1);
            _grid.Children.Add(cbo2);


            //RichTextEditor
            RichTextEditor = new XamRichTextEditor();
            RichTextEditor.Name = "_rte";
            RichTextEditor.AllowDocumentViewSplitting = false;
            RichTextEditor.BorderThickness = new System.Windows.Thickness(0);
            RichTextEditor.Margin = new System.Windows.Thickness(5, 0, 5, 10);
            //rte.ClipboardSerializationProviders.Add(new RtfSerializationProvider());

            RtfDocumentAdapter docAdapter = new RtfDocumentAdapter();

            _rtePlaceholder.Children.Add(docAdapter);
            _rtePlaceholder.Children.Add(RichTextEditor);

            Binding docBinding = new Binding();
            docBinding.Source = RichTextEditor;
            docBinding.Path = new System.Windows.PropertyPath("Document");

            docAdapter.SetBinding(RtfDocumentAdapter.DocumentProperty, docBinding);

            Binding valueBinding = new Binding();
            valueBinding.Source = DataContext;
            valueBinding.Path = new System.Windows.PropertyPath("Message.Body");

            docAdapter.SetBinding(RtfDocumentAdapter.ValueProperty, valueBinding);
        }
    }
}
