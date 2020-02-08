using System.Collections.Generic;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Tags
{
    public class Tag : ITag
    {
        private Dictionary<string, IContent> _contentByID = new Dictionary<string, IContent>();

        public Tag(string name) => Name = name;

        public string Name { get; }
        public string Category { get; set; }
        public IEnumerable<IContent> Contents => _contentByID.Values;

        public bool HasContent(IContent content) => _contentByID.ContainsKey(content.Name);

        public void AddContent(IContent content) => _contentByID[content.Name] = content;

        public void RemoveContent(string name)
        {
            if (_contentByID.ContainsKey(name))
            {
                _contentByID.Remove(name);
            }
        }

        public void Clear() => _contentByID.Clear();
    }
}
