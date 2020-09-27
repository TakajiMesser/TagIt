using System.Collections.Generic;
using System.Windows.Controls;
using TagIt.Shared.Models.Contents;
using TagIt.ViewModels.Tabs;
using TagIt.WPF.ViewModels;
using TagIt.WPF.Views.Explorers;
using TagIt.WPF.Views.Factories;
using TagIt.WPF.Views.Playlists;
using TagIt.WPF.Views.Tabs;
using TagIt.WPF.Views.Tags;
using TagIt.WPF.Views.Viewers;

namespace TagIt.WPF.Views
{
    public class PanelManager : IPanelFactory
    {
        private MainWindowViewModel _mainWindowViewModel;
        private TabTracker _tabTracker;

        private List<DocumentReaderPanel> _documentReaders = new List<DocumentReaderPanel>();
        private List<ImageViewerPanel> _imageViewers = new List<ImageViewerPanel>();
        private List<AudioListenerPanel> _audioListeners = new List<AudioListenerPanel>();
        private List<VideoPlayerPanel> _videoPlayers = new List<VideoPlayerPanel>();
        private List<BrowserPanel> _browsers = new List<BrowserPanel>();

        private LocalExplorer _localExplorer;
        private DriveExplorer _driveExplorer;
        private TagTreeView _tagTreeView;
        private TagListView _tagListView;
        private PlaylistView _playlistView;

        public PanelManager(MainWindowViewModel mainWindowViewModel, TabTracker tabTracker)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _tabTracker = tabTracker;

            _mainWindowViewModel.TabTracker = _tabTracker;
            _mainWindowViewModel.PanelFactory = this;
        }

        public void InitializePanels()
        {
            /*_modelToolPanel = new ModelToolPanel();
            _cameraToolPanel = new CameraToolPanel();
            _brushToolPanel = new BrushToolPanel();
            _actorToolPanel = new ActorToolPanel();
            _lightToolPanel = new LightToolPanel();
            _volumeToolPanel = new VolumeToolPanel();

            _projectTreePanel = new ProjectTreePanel();
            _libraryPanel = new LibraryPanel();
            _propertyPanel = new PropertyPanel();
            _entityTreePanel = new EntityTreePanel();

            //_mainWindowViewModel.ToolsPanelViewModel = _toolPanel.ViewModel;
            _mainWindowViewModel.ModelToolPanelViewModel = _modelToolPanel.ViewModel;
            _mainWindowViewModel.BrushToolPanelViewModel = _brushToolPanel.ViewModel;

            _mainWindowViewModel.ProjectTreePanelViewModel = _projectTreePanel.ViewModel;
            _mainWindowViewModel.LibraryPanelViewModel = _libraryPanel.ViewModel;
            _mainWindowViewModel.PropertyViewModel = _propertyPanel.ViewModel;
            _mainWindowViewModel.EntityTreePanelViewModel = _entityTreePanel.ViewModel;*/
        }

        public void AddDefaultPanels()
        {
            AddDefaultMainPanels();
            AddDefaultSidePanels();
        }

        public void CreateDocumentReaderPanel(IContent content)
        {
            var documentReader = new DocumentReaderPanel();
            _documentReaders.Add(documentReader);

            CreateContentPanel(documentReader, documentReader.ViewModel, content.Name);
            documentReader.ViewModel.Open(content);
            _mainWindowViewModel.DocumentReaderPanelViewModel = documentReader.ViewModel;
        }

        public void CreateImageViewerPanel(IContent content)
        {
            var imageViewer = new ImageViewerPanel();
            _imageViewers.Add(imageViewer);

            CreateContentPanel(imageViewer, imageViewer.ViewModel, content.Name);
            imageViewer.ViewModel.Open(content);
            _mainWindowViewModel.ImageViewerPanelViewModel = imageViewer.ViewModel;
        }

        public void CreateAudioListenerPanel(IContent content)
        {
            var audioListener = new AudioListenerPanel();
            _audioListeners.Add(audioListener);

            CreateContentPanel(audioListener, audioListener.ViewModel, content.Name);
            audioListener.ViewModel.Open(content);
            _mainWindowViewModel.AudioListenerPanelViewModel = audioListener.ViewModel;
        }

        public void CreateVideoPlayerPanel(IContent content)
        {
            var videoPlayer = new VideoPlayerPanel();
            _videoPlayers.Add(videoPlayer);

            CreateContentPanel(videoPlayer, videoPlayer.ViewModel, content.Name);
            videoPlayer.ViewModel.Open(content);
            _mainWindowViewModel.VideoPlayerPanelViewModel = videoPlayer.ViewModel;
        }

        public void CreateBrowserPanel(IContent content)
        {
            var browser = new BrowserPanel();
            _browsers.Add(browser);

            CreateContentPanel(browser, browser.ViewModel, content.Name);
            browser.ViewModel.Open(content);
            _mainWindowViewModel.BrowserPanelViewModel = browser.ViewModel;
        }

        private void CreateContentPanel(Panel panel, PanelViewModel viewModel, string header)
        {
            var tab = new TabItem()
            {
                Header = header,
                Content = panel
            };

            _tabTracker.AddToMainTabs(tab, viewModel);
            viewModel.IsActive = true;
        }

        private void CreateSidePanel(Panel panel, PanelViewModel viewModel, string header)
        {
            var tab = new TabItem()
            {
                Header = header,
                Content = panel
            };

            _tabTracker.AddToSideTabs(tab, viewModel);
        }

        public void OpenLocalExplorerPanel()
        {
            if (!_tabTracker.ContainsSideTab(_localExplorer?.ViewModel))
            {
                _localExplorer = new LocalExplorer();
                _mainWindowViewModel.LocalExplorerViewModel = _localExplorer.ViewModel;
                CreateSidePanel(_localExplorer, _localExplorer.ViewModel, "Local");
            }

            _localExplorer.ViewModel.IsActive = true;
        }

        public void OpenDriveExplorerPanel()
        {
            if (!_tabTracker.ContainsSideTab(_driveExplorer?.ViewModel))
            {
                _driveExplorer = new DriveExplorer();
                _mainWindowViewModel.DriveExplorerViewModel = _driveExplorer.ViewModel;
                CreateSidePanel(_driveExplorer, _driveExplorer.ViewModel, "Drive");
            }

            _driveExplorer.ViewModel.IsActive = true;
        }

        public void OpenTagTreePanel()
        {
            if (!_tabTracker.ContainsSideTab(_tagTreeView?.ViewModel))
            {
                _tagTreeView = new TagTreeView();
                _mainWindowViewModel.TagTreeViewModel = _tagTreeView.ViewModel;
                CreateSidePanel(_tagTreeView, _tagTreeView.ViewModel, "Tags");
            }

            _tagTreeView.ViewModel.IsActive = true;
        }

        public void OpenTagListPanel()
        {
            if (!_tabTracker.ContainsSideTab(_tagListView?.ViewModel))
            {
                _tagListView = new TagListView();
                _mainWindowViewModel.TagListViewModel = _tagListView.ViewModel;
                CreateSidePanel(_tagListView, _tagListView.ViewModel, "Categories");
            }

            _tagListView.ViewModel.IsActive = true;
        }

        public void OpenPlaylistPanel()
        {
            if (!_tabTracker.ContainsSideTab(_playlistView?.ViewModel))
            {
                _playlistView = new PlaylistView();
                _mainWindowViewModel.PlaylistViewModel = _playlistView.ViewModel;
                CreateSidePanel(_playlistView, _playlistView.ViewModel, "Playlists");
            }

            _playlistView.ViewModel.IsActive = true;
        }

        private void AddDefaultMainPanels()
        {
            //_dockTracker.AddToLeftDock(_toolsPanel, _toolsPanel.ViewModel);
            /*_dockTracker.AddToLeftDock(_modelToolPanel, _modelToolPanel.ViewModel);
            _dockTracker.AddToLeftDock(_cameraToolPanel, _cameraToolPanel.ViewModel);
            _dockTracker.AddToLeftDock(_brushToolPanel, _brushToolPanel.ViewModel);
            _dockTracker.AddToLeftDock(_actorToolPanel, _actorToolPanel.ViewModel);
            _dockTracker.AddToLeftDock(_lightToolPanel, _lightToolPanel.ViewModel);
            _dockTracker.AddToLeftDock(_volumeToolPanel, _volumeToolPanel.ViewModel);*/
        }

        private void AddDefaultSidePanels()
        {
            OpenLocalExplorerPanel();
            OpenDriveExplorerPanel();
            OpenTagTreePanel();
            OpenTagListPanel();
            OpenPlaylistPanel();

            _localExplorer.ViewModel.IsActive = true;
        }
    }
}
