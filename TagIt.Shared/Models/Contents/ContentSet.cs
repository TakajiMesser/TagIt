using System.Collections.Generic;
using System.Linq;

namespace TagIt.Shared.Models.Contents
{
    public class ContentSet<T> : IContentSet where T : IContent
    {
        private List<T> _contents = new List<T>();

        public ContentSet(string name) => Name = name;

        public string Name { get; }
        public IContentSet Parent { get; set; }

        public IEnumerable<T> Children => _contents;
        public int Count => _contents.Count;

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
