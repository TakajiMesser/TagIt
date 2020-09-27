using System.Windows;
using System.Windows.Controls;
using TagIt.Shared.Models.Tags;
using TagIt.WPF.Helpers;
using TagIt.WPF.ViewModels.Contents;
using TagIt.WPF.ViewModels.Playlists;
using TagIt.WPF.Views.Tags;

namespace TagIt.WPF.ViewModels.Tags.Trees
{
    public class TaggedViewModel : ViewModel
    {
        private ITag _tag;
        private IContentController _contentController;
        private IPlaylistController _playlistController;
        private ITagRearranger _tagRearranger;
        
        //private List<TagVideoViewModel> _tagLeaves = new List<TagVideoViewModel>();
        private RelayCommand _dragCommand;
        //private RelayCommand _dropCommand;

        public TaggedViewModel(ITag tag, IContentController contentController, ITagRearranger tagRearranger)
        {
            _tag = tag;
            _contentController = contentController;
            _tagRearranger = tagRearranger;

            ContextMenu = new ContextMenu();
            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Add " + Name,
                Command = CreateCommand
            });

            GetInfo();

            /*foreach (var video in _tag.Videos)
            {
                _tagLeaves.Add(new TagVideoViewModel(video, _tag, _videoController, _playlistController, _tagRearranger));
            }

            Children = new ReadOnlyCollection<TagVideoViewModel>(_tagLeaves);*/
        }

        private void GetInfo()
        {
            Name = _tag.Name;
        }

        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        //public ReadOnlyCollection<TagVideoViewModel> Children { get; set; }

        public RelayCommand MenuCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }

        public ContextMenu ContextMenu { get; set; }

        public RelayCommand DragCommand => _dragCommand ?? (_dragCommand = new RelayCommand(
            p =>
            {
                var draggedItem = ViewHelper.FindAncestor<TreeViewItem>((DependencyObject)p);
                //var dataItem = draggedItem.ItemContainerGenerator.ItemFromContainer(null);

                DragDrop.DoDragDrop(draggedItem, Name, DragDropEffects.Move);
            },
            p => true
        ));

        /*public RelayCommand DropCommand => _dropCommand ?? (_dropCommand = new RelayCommand(
            p =>
            {
                var args = (DragEventArgs)p;

                if (/*args.OriginalSource is TagVideoViewModel && *args.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    var videoName = args.Data.GetData(DataFormats.StringFormat) as string;
                    _tagRearranger.RearrangeVideo(Name, videoName, args);


                    /*var originalSource = args.OriginalSource;
                    var source = args.Source;
                    var position = args.GetPosition(null);*
                    //_rearranger.Rearrange((string)name, args);
                }
            },
            p => true
        ));*/
    }
}
