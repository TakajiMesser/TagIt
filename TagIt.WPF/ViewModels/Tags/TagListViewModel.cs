using System.Collections.Generic;
using System.Collections.ObjectModel;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Tags;
using TagIt.WPF.Models.Videos;

namespace TagIt.WPF.ViewModels.Tags
{
    public class TagListViewModel : ViewModel
    {
        private IVideoPlayer _videoPlayer;
        private List<TagViewModel> _tags = new List<TagViewModel>();

        private RelayCommand _addCommand;

        public ObservableCollection<TagViewModel> Tags { get; set; }

        public string NewTagName { get; set; }

        public RelayCommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand(
            p =>
            {
                if (!string.IsNullOrEmpty(NewTagName))
                {
                    var video = _videoPlayer.Video;

                    if (!TagManager.Instance.HasTag(NewTagName))
                    {
                        var tag = TagManager.Instance.CreateTag(NewTagName);
                        tag.AddContent(video);
                        _tags.Add(new TagViewModel(tag.Name, video));
                    }
                    else
                    {
                        TagManager.Instance.TagContent(NewTagName, video);
                    }

                    Tags = new ObservableCollection<TagViewModel>(_tags);
                }

                NewTagName = "";
            },
            p => true
        ));

        public void SetVideoPlayer(IVideoPlayer videoPlayer)
        {
            _videoPlayer = videoPlayer;
            _videoPlayer.Opened += (s, args) =>
            {
                UpdateFromModel(_videoPlayer.Video);
            };
        }

        public void UpdateFromModel(IContent content)
        {
            _tags.Clear();

            foreach (var tag in TagManager.Instance.Tags)
            {
                _tags.Add(new TagViewModel(tag.Name, content));
            }

            Tags = new ObservableCollection<TagViewModel>(_tags);
        }
    }
}
