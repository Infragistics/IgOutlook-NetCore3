using IgOutlook.Business;
using IgOutlook.Core;
using IgOutlook.Core.Controls;
using IgOutlook.Modules.Mail.Resources;
using Infragistics.Controls.Menus;
using System;
using System.Linq;

namespace IgOutlook.Modules.Mail.OutlookGroups
{
    /// <summary>
    /// Interaction logic for MailGroup.xaml
    /// </summary>
    public partial class MailGroup : IOutlookBarGroup
    {
        INavigationItem _selectedItem;

        public string DefaultNavigationPath
        {
            get
            {
                var item = _xamDataTree.SelectionSettings.SelectedNodes[0] as XamDataTreeNode;

                if (item != null)
                    return ((INavigationItem)item.Data).NavigationPath;
                else
                {
                    var inboxItem = (DataContext as MailGroupViewModel).Items.SelectMany(x => x.Items).FirstOrDefault(x => x.Caption == ResourceStrings.MailGroup_Inbox_Text);
                    _selectedItem = inboxItem;
                    return inboxItem != null ? inboxItem.NavigationPath : String.Empty;
                }
            }
        }

        public MailGroup()
        {
            InitializeComponent();
            _xamDataTree.Loaded += _xamDataTree_Loaded;
        }

        void _xamDataTree_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _xamDataTree.Loaded -= _xamDataTree_Loaded;

            var node = XamDataTreeProperties.FindTreeNodeFromDataItem(_xamDataTree, _selectedItem);
            if (node != null)
               node.IsSelected = true;
        }

        private void ActiveNodeChanging(object sender, ActiveNodeChangingEventArgs e)
        {
            _selectedItem = e.NewActiveTreeNode.Data as INavigationItem;
            if (_selectedItem != null && !_selectedItem.CanNavigate)
                e.Cancel = true;
        }
    }
}
