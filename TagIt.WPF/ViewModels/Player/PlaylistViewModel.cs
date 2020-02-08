using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Playlists;
using TagIt.WPF.ViewModels.Contents;
using TagIt.WPF.ViewModels.Custom;
using TagIt.WPF.ViewModels.Explorers;

namespace TagIt.WPF.ViewModels.Player
{
    public class PlaylistViewModel : ViewModel, IPlaylistController
    {
        private IContentController _contentController;
        private List<ContentViewModel> _children = new List<ContentViewModel>();
        private List<IImageButton> _selectedChildren = new List<IImageButton>();

        public IPlaylist Playlist { get; set; }

        public ObservableCollection<ContentViewModel> Children { get; set; }
        public IEnumerable<IImageButton> SelectedChildren
        {
            get => _selectedChildren;
            set
            {
                _selectedChildren.Clear();
                _selectedChildren.AddRange(value);
            }
        }

        public void SetContentController(IContentController contentController) => _contentController = contentController;

        public void OnPlaylistChanged()
        {
            var videos = GetContents();
            SwapChildren(videos);
        }

        private IEnumerable<ContentViewModel> GetContents()
        {
            foreach (var video in Playlist.Contents)
            {
                yield return new ContentViewModel(video, _contentController);
            }
        }

        public void AddContent(IContent content)
        {
            Playlist?.Contents.Add(content);
            var contents = GetContents();
            SwapChildren(contents);
        }

        private void SwapChildren(IEnumerable<ContentViewModel> items)
        {
            // TODO - Is this still necessary?
            items = items.ToList();

            _children.Clear();
            _children.AddRange(items);

            Children = new ObservableCollection<ContentViewModel>(_children);
        }
    }
}
