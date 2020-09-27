using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Local;
using TagIt.Shared.Models.Tags;
using TagIt.WPF.Helpers;
using TagIt.WPF.Models.Thumbnails;
using TagIt.WPF.Properties;
using TagIt.WPF.ViewModels.Contents;

namespace TagIt.WPF.ViewModels.Explorers
{
    public class ContentViewModel : ImageButtonViewModel
    {
        private IContentController _contentController;
        private IExplorer _explorer;

        private RelayCommand _openCommand;
        private RelayCommand _addToPlaylistCommand;
        private RelayCommand _toggleTagCommand;

        public ContentViewModel(IContent content, IContentController contentController, IExplorer explorer = null) : base(content.Name)
        {
            Content = content;
            _contentController = contentController;
            _explorer = explorer;

            foreach (var menuItem in GetMenuItems())
            {
                ContextMenu.Items.Add(menuItem);
            }

            GetOrGenerateThumbnail();
        }

        public IContent Content { get; }

        

        public async void GetOrGenerateThumbnail()
        {
            if (Content.Kind == Kinds.Folder)
            {
                Image = ImageHelper.FastConvertToBitmapSource(Resources.baseline_work_black_36dp);
            }
            else if (Content.Kind == Kinds.Document)
            {
                Image = ImageHelper.FastConvertToBitmapSource(Resources.baseline_description_black_36dp);
            }
            else if (Content.Kind == Kinds.Image)
            {
                if (Content is ILocalContent)
                {
                    Image = ImageHelper.ConvertToBitmapSource(ImageHelper.GetThumbnail(Content.Path, 100, 100, ImageHelper.ThumbnailOptions.InCacheOnly));
                }
                else
                {
                    Image = ImageHelper.FastConvertToBitmapSource(Resources.baseline_contact_support_black_36dp);
                }
            }
            else if (Content.Kind == Kinds.Video)
            {
                Image = ImageHelper.FastConvertToBitmapSource(Resources.baseline_visibility_black_36dp);
            }
            else if (Content.Kind == Kinds.Audio)
            {
                Image = ImageHelper.FastConvertToBitmapSource(Resources.baseline_volume_up_black_36dp);
            }
            else
            {
                var preview = await _contentController.PreviewProvider.GetPreview(Content);

                if (_contentController.PreviewProvider.ThumbnailGenerator is ThumbnailGenerator thumbnailGenerator)
                {
                    var image = thumbnailGenerator.GetThumbnailImage(preview.Path);

                    if (image != null)
                    {
                        Image = image;
                        //InvokePropertyChanged(nameof(Icon));
                    }
                }
            }
        }

        public ContextMenu ContextMenu { get; } = new ContextMenu();

        public override RelayCommand SelectCommand => null;

        public override RelayCommand OpenCommand => _openCommand ?? (_openCommand = new RelayCommand(
            p =>
            {
                if (Content is IContentSet contentSet)
                {
                    _explorer.OpenContentSet(contentSet);
                }
                else
                {
                    _contentController.OpenContent(Content);
                }
            },
            p => true
        ));

        public override RelayCommand DragCommand => null;

        public RelayCommand AddToPlaylistCommand => _addToPlaylistCommand ?? (_addToPlaylistCommand = new RelayCommand(
            p => _contentController.AddToPlaylist(Content),
            p => true
        ));

        public RelayCommand ToggleTagCommand => _toggleTagCommand ?? (_toggleTagCommand = new RelayCommand(
            p =>
            {
                // TODO - Get tag state on this current video
                // Get all selected videos
                // Contact tagprovider to toggle on/off this tag for all selected videos (based on state of current video)
                if (p is ITag tag)
                {
                    var isTagged = TagManager.Instance.IsTagged(tag.Name, Content);
                    var selectedContent = _explorer.SelectedChildren.OfType<ContentViewModel>();

                    if (isTagged)
                    {
                        foreach (var content in selectedContent)
                        {
                            TagManager.Instance.UntagContent(tag.Name, content.Content);
                        }
                    }
                    else
                    {
                        foreach (var content in selectedContent)
                        {
                            TagManager.Instance.TagContent(tag.Name, content.Content);
                        }
                    }
                }
            },
            p => true
        ));

        protected /*override*/ IEnumerable<MenuItem> GetMenuItems()
        {
            var menuItem = new MenuItem()
            {
                Header = "Add/Remove Tags"
            };

            foreach (var subMenuItem in GetTagMenuItems())
            {
                menuItem.Items.Add(subMenuItem);
            }

            yield return menuItem;

            yield return new MenuItem()
            {
                Header = "Add To Playlist",
                Command = AddToPlaylistCommand
            };
        }

        private IEnumerable<MenuItem> GetTagMenuItems()
        {
            foreach (var tag in TagManager.Instance.Tags)
            {
                yield return new MenuItem()
                {
                    Header = tag.Name,
                    IsChecked = tag.HasContent(Content),
                    Command = ToggleTagCommand,
                    CommandParameter = tag
                };
            }
        }
    }
}