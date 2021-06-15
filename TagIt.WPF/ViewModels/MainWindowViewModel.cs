using System;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Local;
using TagIt.Shared.Models.Previews;
using TagIt.Shared.Models.Remote.Drive;
using TagIt.Shared.Models.Tags;
using TagIt.WPF.Helpers;
using TagIt.WPF.Models.Thumbnails;
using TagIt.WPF.ViewModels.Contents;
using TagIt.WPF.ViewModels.Explorers;
using TagIt.WPF.ViewModels.Playlists;
using TagIt.WPF.ViewModels.Settings;
using TagIt.WPF.ViewModels.Tags;
using TagIt.WPF.ViewModels.Tags.Trees;
using TagIt.WPF.ViewModels.Viewers;
using TagIt.WPF.Views.Factories;
using TagIt.WPF.Views.Tabs;

namespace TagIt.WPF.ViewModels
{
    public class MainWindowViewModel : WindowViewModel, IContentController
    {
        //private FileLibraryManager _libraryManager;
        //private VideoManager _videoManager;
        //private ThumbnailManager _thumbnailManager;

        //private LocalVideo _testVideoFile;
        //private VideoBookmark _testVideoBookmark;
        private ContentManager _contentManager;
        private PreviewManager _previewManager;

        private RelayCommand _playPauseCommand;
        private RelayCommand _skipForwardCommand;
        private RelayCommand _skipBackwardCommand;

        public MainWindowViewModel() => Title = "MainWindow";

        public IWindowFactory WindowFactory { get; set; }
        public ITabTracker TabTracker { get; set; }
        public IPanelFactory PanelFactory { get; set; }

        public DocumentReaderPanelViewModel DocumentReaderPanelViewModel { get; set; }
        public ImageViewerPanelViewModel ImageViewerPanelViewModel { get; set; }
        public AudioListenerPanelViewModel AudioListenerPanelViewModel { get; set; }
        public VideoPlayerPanelViewModel VideoPlayerPanelViewModel { get; set; }
        public BrowserPanelViewModel BrowserPanelViewModel { get; set; }

        public LocalExplorerViewModel LocalExplorerViewModel { get; set; }
        public DriveExplorerViewModel DriveExplorerViewModel { get; set; }
        public TagTreeViewModel TagTreeViewModel { get; set; }
        public TagListViewModel TagListViewModel { get; set; }
        public PlaylistViewModel PlaylistViewModel { get; set; }

        public SettingsWindowViewModel SettingsWindowViewModel { get; set; }

        public void Initialize()
        {
            _contentManager = new ContentManager(TagIt.Shared.Helpers.FilePathHelper.CACHE_DIRECTORY);

            var localFetcher = new LocalFetcher(FilePathHelper.INITIAL_LOCAL_DIRECTORY);
            var driveFetcher = new DriveFetcher();

            _contentManager.AddFetcher(localFetcher);
            _contentManager.AddFetcher(driveFetcher);

            var thumbnailGenerator = new ThumbnailGenerator();
            _previewManager = new PreviewManager(thumbnailGenerator);

            LocalExplorerViewModel.UpdateFromModel(this, localFetcher);
            DriveExplorerViewModel.UpdateFromModel(this, driveFetcher);
        }

        public RelayCommand PlayPauseCommand => _playPauseCommand ?? (_playPauseCommand = new RelayCommand(
            p => VideoPlayerPanelViewModel?.PlayPauseCommand.Execute(),
            p => true
        ));

        public RelayCommand SkipForwardCommand => _skipForwardCommand ?? (_skipForwardCommand = new RelayCommand(
            p => VideoPlayerPanelViewModel?.SkipForwardCommand.Execute(),
            p => true
        ));

        public RelayCommand SkipBackwardCommand => _skipBackwardCommand ?? (_skipBackwardCommand = new RelayCommand(
            p => VideoPlayerPanelViewModel?.SkipBackwardCommand.Execute(),
            p => true
        ));

        public IContent CurrentContent { get; private set; }
        public IPreviewProvider PreviewProvider => _previewManager;

        public event EventHandler<ContentEventArgs> ContentChanged;

        public void OpenContent(IContent content)
        {
            switch (content.Kind)
            {
                case Kinds.Document:
                    PanelFactory.CreateDocumentReaderPanel(content);
                    break;
                case Kinds.Image:
                    PanelFactory.CreateImageViewerPanel(content);
                    break;
                case Kinds.Audio:
                    PanelFactory.CreateAudioListenerPanel(content);
                    break;
                case Kinds.Video:
                    PanelFactory.CreateVideoPlayerPanel(content);
                    break;
                case Kinds.Bookmark:
                    PanelFactory.CreateBrowserPanel(content);
                    break;
            }

            CurrentContent = content;
            ContentChanged?.Invoke(this, new ContentEventArgs(content));
        }

        public void AddToPlaylist(IContent content) => PlaylistViewModel?.AddContent(content);

        /*public void OpenVideo(IVideo video)
        {
            switch (video)
            {
                case VideoFile videoFile:
                    VideoPlayerViewModel.OpenVideo(videoFile);
                    break;
                case DriveFile driveFile:
                    VideoPlayerViewModel.OpenVideo(driveFile);
                    break;
                case VideoBookmark videoBookmark:
                    BrowserViewModel.OpenVideo(videoBookmark);
                    break;
            }
        }*/

        public void SaveOnClose() => TagManager.Instance.SaveToManifest(FilePathHelper.TAG_LIBRARY_PATH);

        public void OnTagTreeViewModelChanged()
        {
            if (VideoPlayerPanelViewModel != null)
            {
                TagTreeViewModel.UpdateFromModel(this);
            }
        }

        //public void OnLocalExplorerViewModelChanged() => LocalExplorerViewModel.UpdateFromModel(this);

        //public void OnDriveExplorerViewModelChanged() => DriveExplorerViewModel.UpdateFromModel(this);

        public void OnBrowserViewModelChanged()
        {
            //BrowserViewModel.OpenVideo(_testVideoBookmark);
        }

        public void OnVideoPlayerPanelViewModelChanged()
        {
            if (TagListViewModel != null)
            {
                TagListViewModel.SetVideoPlayer(VideoPlayerPanelViewModel.VideoPlayer);
            }
        }

        public void OnPlaylistViewModelChanged() => PlaylistViewModel.SetContentController(this);

        public void OnTagListViewModelChanged()
        {
            if (VideoPlayerPanelViewModel != null)
            {
                TagListViewModel.SetVideoPlayer(VideoPlayerPanelViewModel.VideoPlayer);
            }
        }

        public void OpenLibraries()
        {
            //VideoExplorerViewModel.UpdateFromModel(_libraryManager, VideoPlayerViewModel);
        }
    }
}
