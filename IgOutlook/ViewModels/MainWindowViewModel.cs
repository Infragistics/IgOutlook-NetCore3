using IgOutlook.Core;
using IgOutlook.Core.Commands;
using IgOutlook.Core.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;

namespace IgOutlook.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        IRegionManager _regionManager;

        string _applicationTitle = "IG Outlook";

        private int _itemsCount;
        public int ItemsCount
        {
            get { return _itemsCount; }
            set { SetProperty(ref _itemsCount, value); }
        }

        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(ExecuteNavigateCommand));

        private DelegateCommand _exitCommand;
        public DelegateCommand ExitCommand =>
            _exitCommand ?? (_exitCommand = new DelegateCommand(ExecuteExitCommand));

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IApplicationCommands applicationCommands)
        {
            Title = _applicationTitle;

            _regionManager = regionManager;

            eventAggregator.GetEvent<ViewActivateEvent>().Subscribe(ViewActivated);
            eventAggregator.GetEvent<ViewItemsCountChangedEvent>().Subscribe(ViewItemsCountChanged);

            applicationCommands.NavigateCommand.RegisterCommand(NavigateCommand);
        }

        void ExecuteNavigateCommand(string navigationPath)
        {
            if (string.IsNullOrWhiteSpace(navigationPath))
                return;

            if (!string.IsNullOrWhiteSpace(navigationPath))
                _regionManager.RequestNavigate(RegionNames.ContentRegion, navigationPath);
        }

        void ExecuteExitCommand()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ViewActivated(string viewName)
        {
            Title = String.Format("{0} - {1}", viewName, _applicationTitle);
        }

        private void ViewItemsCountChanged(int count)
        {
            ItemsCount = count;
        }
    }
}
