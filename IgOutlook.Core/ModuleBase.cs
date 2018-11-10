using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace IgOutlook.Core
{
    public class ModuleBase : IModule
    {
        protected IRegionManager RegionManager { get; private set; }

        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
            RegionManager = containerProvider.Resolve<IRegionManager>();
            ResolveOutlookGroup();
        }

        public virtual void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        protected virtual void ResolveOutlookGroup()
        {

        }
    }
}
