using System.Collections.Generic;

namespace TagIt.Shared.Models.Contents
{
    public abstract class ContentFetcher<T> : IFetcher where T : IContent
    {
        protected Dictionary<string, T> _contentByID = new Dictionary<string, T>();
        protected List<string> _rootIDs = new List<string>();

        public IEnumerable<T> RootContents
        {
            get
            {
                foreach (var id in _rootIDs)
                {
                    yield return _contentByID[id];
                }
            }
        }

        public T GetContent(string id) => _contentByID[id];
        public bool HasContent(string id) => _contentByID.ContainsKey(id);

        public abstract void Fetch();

        public void Clear()
        {
            _contentByID.Clear();
            _rootIDs.Clear();
        }
    }
}
