using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using TagIt.WPF.Views.Factories;
using TagIt.WPF.Views.Settings;

namespace TagIt.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWindowFactory, IWindow
    {
        private SettingsWindow _settingsWindow;

        public MainWindow()
        {
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;

            InitializeComponent();

            Menu.ViewModel.WindowFactory = this;
            Menu.ViewModel.ContentController = ViewModel;

            ViewModel.Window = this;

            ViewModel.VideoPlayerViewModel = Player.ViewModel;
            ViewModel.BrowserViewModel = Browser.ViewModel;

            ViewModel.LocalExplorerViewModel = LocalExplorer.ViewModel;
            ViewModel.DriveExplorerViewModel = DriveExplorer.ViewModel;
            ViewModel.TagTreeViewModel = TagTree.ViewModel;
            ViewModel.TagListViewModel = TagListView.ViewModel;
            ViewModel.PlaylistViewModel = PlaylistView.ViewModel;

            ViewModel.OpenLibraries();
        }

        private bool _isFullscreen = false;

        public void ToggleFullscreen()
        {
            if (_isFullscreen)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;

                var mediaElement = Content as UIElement;
                Content = WindowPanel;
                Player.RootGrid.Children.Add(mediaElement);

                _isFullscreen = false;
            }
            else
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;

                var mediaElement = Player.Player;
                Player.RootGrid.Children.Remove(mediaElement);
                Content = mediaElement;

                _isFullscreen = true;
            }
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
