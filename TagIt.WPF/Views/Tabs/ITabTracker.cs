using TagIt.ViewModels.Tabs;

namespace TagIt.WPF.Views.Tabs
{
    public interface ITabTracker
    {
        PanelViewModel ActiveMainTabVM { get; set; }
        PanelViewModel ActiveSideTabVM { get; set; }
    }
}
