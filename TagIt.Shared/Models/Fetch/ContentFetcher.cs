using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Fetch
{
    public abstract class ContentFetcher<T> : IFetcher where T : IContent
    {
        protected Dictionary<string, T> _contentByPath = new Dictionary<string, T>();
        protected List<string> _rootPaths = new List<string>();

        public ContentFetcher(string name) => Name = name;

        public string Name { get; }

        public IEnumerable<T> RootContents
        {
            get
            {
                foreach (var path in _rootPaths)
                {
                    yield return _contentByPath[path];
                }
            }
        }

        public event EventHandler<ContentEventArgs> ContentAdded;

        protected void AddContent(T content)
        {
            _contentByPath.Add(content.Path, content);
            ContentAdded?.Invoke(this, new ContentEventArgs(content));
        }

        public T GetContentOrDefault(string path) => HasContent(path) ? GetContent(path) : default;
        public T GetContent(string path) => _contentByPath[path];
        public bool HasContent(string path) => _contentByPath.ContainsKey(path);

        public abstract Task Fetch();

        public void Clear()
        {
            _contentByPath.Clear();
            _rootPaths.Clear();
        }
    }
}
