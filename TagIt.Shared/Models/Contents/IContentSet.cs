using System.Collections.Generic;

namespace TagIt.Shared.Models.Contents
{
    public interface IContentSet : IContent { }
    public interface IContentSet<T> : IContentSet where T : IContent
    {
        IEnumerable<T> Contents { get; }
        int ContentCount { get; }

        void AddContent(T content);
        T GetContent(int index);
        T GetContent(string name);
        void Clear();
    }
}
