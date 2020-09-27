using System.Collections.Generic;
using System.Collections.ObjectModel;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Tags;
using TagIt.Shared.Models.Viewers;
using TagIt.ViewModels.Tabs;

namespace TagIt.WPF.ViewModels.Tags
{
    public class TagListViewModel : PanelViewModel
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
                    var content = _videoPlayer.Content;

                    if (!TagManager.Instance.HasTag(NewTagName))
                    {
                        var tag = TagManager.Instance.CreateTag(NewTagName);
                        tag.AddContent(content);
                        _tags.Add(new TagViewModel(tag.Name, content));
                    }
                    else
                    {
                        TagManager.Instance.TagContent(NewTagName, content);
                    }

                    Tags = new ObservableCollection<TagViewModel>(_tags);
                }

                NewTagName = "";
            },
            p => _videoPlayer != null && _videoPlayer.Content != null
        ));

        public void SetVideoPlayer(IVideoPlayer videoPlayer)
        {
            _videoPlayer = videoPlayer;
            _videoPlayer.Opened += (s, args) =>
            {
                UpdateFromModel(_videoPlayer.Content);
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
