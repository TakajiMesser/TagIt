using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagIt.WPF.Models.Explorers;
using TagIt.WPF.Models.Videos;
using TagIt.WPF.ViewModels.Player;
using TagIt.WPF.Views;

namespace TagIt.WPF.ViewModels.Explorers
{
    /*public class FolderViewModel : ImageButtonViewModel
    {
        private IExplorer _libraryTracker;

        private RelayCommand _openCommand;

        public FolderViewModel(IFolderInfo folderInfo, IPlaylistController playlistController, IVideoController videoController, IExplorer libraryTracker) : base(folderInfo.Name)
        {
            _libraryTracker = libraryTracker;
            Children = new ReadOnlyCollection<ImageButtonViewModel>(CreateChildren(folderInfo.Items, playlistController, videoController, libraryTracker).ToList());
        }

        public ReadOnlyCollection<ImageButtonViewModel> Children { get; set; }

        public override RelayCommand SelectCommand => null;

        public override RelayCommand OpenCommand => _openCommand ?? (_openCommand = new RelayCommand(
            p => _libraryTracker.OpenLibrary(Name, Children),
            p => true
        ));

        public override RelayCommand DragCommand => null;

        public static IEnumerable<ImageButtonViewModel> CreateChildren(IEnumerable<IPathInfo> pathInfos, IPlaylistController playlistController, IVideoController videoController, IExplorer libraryTracker)
        {
            foreach (var item in pathInfos)
            {
                if (item is IFolderInfo folderInfo)
                {
                    yield return new FolderViewModel(folderInfo, playlistController, videoController, libraryTracker);
                }
                else if (item is IVideo video)
                {
                    yield return new VideoViewModel(video, playlistController, videoController, libraryTracker);
                }
            }
        }

        public static IEnumerable<ImageButtonViewModel> FilterChildren(IEnumerable<IPathInfo> pathInfos, IPlaylistController playlistController, IVideoController videoController, IExplorer libraryTracker, Predicate<IPathInfo> predicate)
        {
            foreach (var item in pathInfos)
            {
                if (predicate(item))
                {
                    if (item is IFolderInfo folderInfo)
                    {
                        yield return new FolderViewModel(folderInfo, playlistController, videoController, libraryTracker);
                    }
                    else if (item is IVideo video)
                    {
                        yield return new VideoViewModel(video, playlistController, videoController, libraryTracker);
                    }
                }
            }
        }
    }*/
}