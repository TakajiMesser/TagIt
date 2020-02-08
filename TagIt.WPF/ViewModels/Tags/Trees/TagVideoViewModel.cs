using System.Windows;
using System.Windows.Controls;
using TagIt.WPF.Helpers;
using TagIt.WPF.Models.Tags;
using TagIt.WPF.Models.Videos;
using TagIt.WPF.ViewModels.Player;
using TagIt.WPF.Views;
using TagIt.WPF.Views.Tags;

namespace TagIt.WPF.ViewModels.Tags.Trees
{
    /*public class TagVideoViewModel : ViewModel
    {
        private IVideo _video;
        private ITag _tag;
        private IVideoController _videoController;
        private ITagRearranger _tagRearranger;
        private IPlaylistController _playlistController;

        private RelayCommand _openCommand;
        private RelayCommand _addToPlaylistCommand;
        private RelayCommand _removeCommand;
        private RelayCommand _dragCommand;

        public TagVideoViewModel(IVideo video, ITag tag, IVideoController videoController, IPlaylistController playlistController, ITagRearranger tagRearranger)
        {
            _video = video;
            _tag = tag;
            _videoController = videoController;
            _tagRearranger = tagRearranger;
            _playlistController = playlistController;

            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Open",
                Command = OpenCommand
            });
            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Add To Playlist",
                Command = AddToPlaylistCommand
            });
            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Remove",
                Command = RemoveCommand
            });
        }

        public string Name => _video.Name;

        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }

        public RelayCommand OpenCommand => _openCommand ?? (_openCommand = new RelayCommand(
            p => _videoController.OpenVideo(_video),
            p => true
        ));

        public RelayCommand AddToPlaylistCommand => _addToPlaylistCommand ?? (_addToPlaylistCommand = new RelayCommand(
            p => _playlistController.AddVideo(_video),
            p => true
        ));

        public RelayCommand RemoveCommand => _removeCommand ?? (_removeCommand = new RelayCommand(
            p => TagManager.Instance.UntagVideo(_tag.Name, _video),
            p => true
        ));

        public RelayCommand DragCommand => _dragCommand ?? (_dragCommand = new RelayCommand(
            p =>
            {
                var draggedItem = ViewHelper.FindAncestor<TreeViewItem>((DependencyObject)p);
                //var dataItem = draggedItem.ItemContainerGenerator.ItemFromContainer(null);

                DragDrop.DoDragDrop(draggedItem, Name, DragDropEffects.Move);
            },
            p => true
        ));

        public ContextMenu ContextMenu { get; set; } = new ContextMenu();
    }*/
}
