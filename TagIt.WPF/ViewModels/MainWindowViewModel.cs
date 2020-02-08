using System;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Contents.Video;
using TagIt.Shared.Models.Tags;
using TagIt.WPF.Helpers;
using TagIt.WPF.ViewModels.Browsers;
using TagIt.WPF.ViewModels.Contents;
using TagIt.WPF.ViewModels.Explorers;
using TagIt.WPF.ViewModels.Player;
using TagIt.WPF.ViewModels.Tags;
using TagIt.WPF.ViewModels.Tags.Trees;
using TagIt.WPF.Views;

namespace TagIt.WPF.ViewModels
{
    public class MainWindowViewModel : ViewModel, IContentController
    {
        //private FileLibraryManager _libraryManager;
        //private VideoManager _videoManager;
        //private ThumbnailManager _thumbnailManager;

        //private LocalVideo _testVideoFile;
        //private VideoBookmark _testVideoBookmark;

        public string Title { get; set; } = "Main Window";

        public IWindow Window { get; set; }

        public PlayerViewModel VideoPlayerViewModel { get; set; }
        public BrowserViewModel BrowserViewModel { get; set; }

        public LocalExplorerViewModel LocalExplorerViewModel { get; set; }
        public DriveExplorerViewModel DriveExplorerViewModel { get; set; }

        public TagTreeViewModel TagTreeViewModel { get; set; }
        public TagListViewModel TagListViewModel { get; set; }
        public PlaylistViewModel PlaylistViewModel { get; set; }

        private RelayCommand _playPauseCommand;
        private RelayCommand _skipForwardCommand;
        private RelayCommand _skipBackwardCommand;
        private RelayCommand _fullscreenCommand;

        public RelayCommand PlayPauseCommand => _playPauseCommand ?? (_playPauseCommand = new RelayCommand(
            p =>
            {
                VideoPlayerViewModel?.PlayPauseCommand.Execute();
            },
            p => true
        ));

        public RelayCommand SkipForwardCommand => _skipForwardCommand ?? (_skipForwardCommand = new RelayCommand(
            p =>
            {
                VideoPlayerViewModel?.SkipForwardCommand.Execute();
            },
            p => true
        ));

        public RelayCommand SkipBackwardCommand => _skipBackwardCommand ?? (_skipBackwardCommand = new RelayCommand(
            p =>
            {
                VideoPlayerViewModel?.SkipBackwardCommand.Execute();
            },
            p => true
        ));

        public RelayCommand FullscreenCommand => _fullscreenCommand ?? (_fullscreenCommand = new RelayCommand(
            p =>
            {
                //VideoPlayerViewModel?.VideoPlayer.MediaElement;
                Window?.ToggleFullscreen();
            },
            p => true
        ));

        public MainWindowViewModel()
        {

        }

        public IContent CurrentContent { get; private set; }
        public event EventHandler<ContentEventArgs> ContentChanged;

        public void OpenContent(IContent content)
        {
            switch (content)
            {
                case IVideo video:
                    VideoPlayerViewModel?.OpenVideo(video);
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

        public void SaveOnClose() => TagManager.Instance.SaveToLibrary(FilePathHelper.TAG_LIBRARY_PATH);

        public void OnTagTreeViewModelChanged()
        {
            if (VideoPlayerViewModel != null)
            {
                TagTreeViewModel.UpdateFromModel(this);
            }
        }

        public void OnLocalExplorerViewModelChanged() => LocalExplorerViewModel.UpdateFromModel(this);

        public void OnDriveExplorerViewModelChanged() => DriveExplorerViewModel.UpdateFromModel(this);

        public void OnBrowserViewModelChanged()
        {
            //BrowserViewModel.OpenVideo(_testVideoBookmark);
        }

        public void OnVideoPlayerViewModelChanged()
        {
            if (TagListViewModel != null)
            {
                TagListViewModel.SetVideoPlayer(VideoPlayerViewModel.VideoPlayer);
            }
        }

        public void OnPlaylistViewModelChanged() => PlaylistViewModel.SetContentController(this);

        public void OnTagListViewModelChanged()
        {
            if (VideoPlayerViewModel != null)
            {
                TagListViewModel.SetVideoPlayer(VideoPlayerViewModel.VideoPlayer);
            }
        }

        public void OpenLibraries()
        {
            //VideoExplorerViewModel.UpdateFromModel(_libraryManager, VideoPlayerViewModel);
        }
    }
}
