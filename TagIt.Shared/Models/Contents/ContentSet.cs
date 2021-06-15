using System.Collections.Generic;
using System.Linq;

namespace TagIt.Shared.Models.Contents
{
    public abstract class ContentSet<T> : Content, IContentSet<T> where T : IContent
    {
        private List<T> _contents = new List<T>();

        public ContentSet(string name, string path) : base(name, path) => Kind = Kinds.Folder;

        public IEnumerable<T> Contents => _contents;
        public int ContentCount => _contents.Count;

        public void AddContent(T content)
        {
            content.Parent = this;
            _contents.Add(content);
        }

        public T GetContent(int index) => _contents[index];
        public T GetContent(string name) => _contents.FirstOrDefault(c => c.Name == name);

        public void Clear() => _contents.Clear();
    }
}
