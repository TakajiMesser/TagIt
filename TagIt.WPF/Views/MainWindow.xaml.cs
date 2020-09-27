using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using TagIt.WPF.Views.Factories;
using TagIt.WPF.Views.Settings;
using TagIt.WPF.Views.Tabs;

namespace TagIt.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWindowFactory, IWindow
    {
        private PanelManager _panelManager;
        private SettingsWindow _settingsWindow;

        public MainWindow()
        {
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            InitializeComponent();

            _panelManager = new PanelManager(ViewModel, new TabTracker(MainTabs, SideTabs));
            _panelManager.InitializePanels();

            Menu.ViewModel.WindowFactory = this;
            Menu.ViewModel.ContentController = ViewModel;

            ViewModel.Window = this;
            //ViewModel.ViewerViewModel = Player.ViewModel;

            ViewModel.LocalExplorerViewModel = LocalExplorer.ViewModel;
            ViewModel.DriveExplorerViewModel = DriveExplorer.ViewModel;
            ViewModel.TagTreeViewModel = TagTree.ViewModel;
            ViewModel.TagListViewModel = TagListView.ViewModel;
            ViewModel.PlaylistViewModel = PlaylistView.ViewModel;

            ViewModel.OpenLibraries();

            ViewModel.Initialize();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModel.SaveOnClose();
            base.OnClosing(e);
            Application.Current.Shutdown();
        }

        public void CreateSettingsWindow()
        {
            _settingsWindow = new SettingsWindow();
            //_settingsWindow.ViewModel.MainView = this;

            //ViewModel.SettingsWindowViewModel = _settingsWindow.ViewModel;
            _settingsWindow.Show();
        }
    }
}
