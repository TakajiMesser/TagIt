using System.Collections.Generic;
using System.Linq;

namespace TagIt.Shared.Models.Contents
{
    public abstract class ContentSet<T> : Content, IContentSet where T : IContent
    {
        private List<T> _children = new List<T>();

        public ContentSet(string name, string path) : base(name, path) => Kind = Kinds.Folder;

        public IEnumerable<T> Children => _children;
        public int ChildCount => _children.Count;

        public void AddChild(T content)
        {
            content.Parent = this;
            _children.Add(content);
        }

        public T GetContent(int index) => _children[index];
        public T GetContent(string name) => _children.FirstOrDefault(c => c.Name == name);

        public void Clear() => _children.Clear();
    }
}
