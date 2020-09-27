using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TagIt.ViewModels.Tabs;

namespace TagIt.WPF.Views.Tabs
{
    public class TabTracker : ITabTracker
    {
        private TabControl _mainTabControl;
        private TabControl _sideTabControl;

        private PanelViewModel _activeMainTabVM;
        private PanelViewModel _activeSideTabVM;

        private Dictionary<PanelViewModel, TabItem> _mainTabViewByVM = new Dictionary<PanelViewModel, TabItem>();
        private Dictionary<PanelViewModel, TabItem> _sideTabViewByVM = new Dictionary<PanelViewModel, TabItem>();
        private Dictionary<TabItem, PanelViewModel> _tabVMByView = new Dictionary<TabItem, PanelViewModel>();

        public TabTracker(TabControl mainTabControl, TabControl sideTabControl)
        {
            _mainTabControl = mainTabControl;
            _sideTabControl = sideTabControl;

            _mainTabControl.SelectionChanged += TabControl_SelectionChanged;
            _sideTabControl.SelectionChanged += TabControl_SelectionChanged;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var tab in e.AddedItems.OfType<TabItem>())
            {
                if (_tabVMByView.ContainsKey(tab))
                {
                    _tabVMByView[tab].IsActive = true;
                }
            }

            foreach (var tab in e.RemovedItems.OfType<TabItem>())
            {
                if (_tabVMByView.ContainsKey(tab))
                {
                    _tabVMByView[tab].IsActive = false;
                }
            }
        }

        public PanelViewModel ActiveMainTabVM
        {
            get => _activeMainTabVM;
            set
            {
                if (!_mainTabViewByVM.ContainsKey(value)) throw new KeyNotFoundException("ViewModel not found in tabs");

                _mainTabViewByVM[value].IsSelected = true;
                _activeMainTabVM = value;
            }
        }

        public PanelViewModel ActiveSideTabVM
        {
            get => _activeSideTabVM;
            set
            {
                if (!_sideTabViewByVM.ContainsKey(value)) throw new KeyNotFoundException("ViewModel not found in tabs");

                _sideTabViewByVM[value].IsSelected = true;
                _activeSideTabVM = value;
            }
        }

        public void AddToMainTabs(TabItem tab, PanelViewModel viewModel)
        {
            viewModel.BecameActive += (s, args) => ActiveMainTabVM = viewModel;

            _mainTabViewByVM.Add(viewModel, tab);
            _tabVMByView.Add(tab, viewModel);
            _mainTabControl.Items.Add(tab);
        }

        public void AddToSideTabs(TabItem tab, PanelViewModel viewModel)
        {
            viewModel.BecameActive += (s, args) => ActiveSideTabVM = viewModel;

            _sideTabViewByVM.Add(viewModel, tab);
            _tabVMByView.Add(tab, viewModel);
            _sideTabControl.Items.Add(tab);
        }

        public bool ContainsMainTab(PanelViewModel viewModel) => viewModel != null
            && _mainTabViewByVM.ContainsKey(viewModel);

        public bool ContainsSideTab(PanelViewModel viewModel) => viewModel != null
            && _sideTabViewByVM.ContainsKey(viewModel);
    }
}
