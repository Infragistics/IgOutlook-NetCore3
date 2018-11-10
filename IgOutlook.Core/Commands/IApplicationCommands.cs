using Prism.Commands;

namespace IgOutlook.Core.Commands
{
    public interface IApplicationCommands
    {
        CompositeCommand NavigateCommand { get; }
    }
}
