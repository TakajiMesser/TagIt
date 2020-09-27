using System.Collections.Generic;
using System.Threading.Tasks;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Fetch
{
    public abstract class ContentFetcher<T> : IFetcher where T : IContent
    {
        private IContentProvider _contentProvider;

        protected Dictionary<string, T> _contentByPath = new Dictionary<string, T>();
        protected List<string> _rootPaths = new List<string>();

        public ContentFetcher(IContentProvider contentProvider, string name)
        {
            _contentProvider = contentProvider;
            Name = name;
        }

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

        protected void AddContent(T content)
        {
            _contentByPath.Add(content.Path, content);
            _contentProvider.AddContent(content);
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
