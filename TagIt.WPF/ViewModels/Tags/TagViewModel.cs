using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Tags;

namespace TagIt.WPF.ViewModels.Tags
{
    public class TagViewModel : ViewModel
    {
        private IContent _content;
        private bool _isTagged = false;

        public TagViewModel(string name, IContent content)
        {
            Name = name;

            _content = content;
            _isTagged = TagManager.Instance.IsTagged(Name, content);
        }
        
        public string Name { get; }
        public bool IsTagged
        {
            get => _isTagged;
            set
            {
                if (value != _isTagged)
                {
                    _isTagged = value;

                    if (value)
                    {
                        TagManager.Instance.TagContent(Name, _content);
                    }
                    else
                    {
                        TagManager.Instance.UntagContent(Name, _content);
                    }
                }
            }
        }
    }
}
