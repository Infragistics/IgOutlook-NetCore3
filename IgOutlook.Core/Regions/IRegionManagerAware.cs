using Prism.Regions;

namespace IgOutlook.Core.Regions
{
    public interface IRegionManagerAware
    {
        IRegionManager RegionManager { get; set; }
    }
}
