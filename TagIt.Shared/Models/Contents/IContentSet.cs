using System.Collections.Generic;

namespace TagIt.Shared.Models.Contents
{
    public interface IContentSet : IContent { }
    public interface IContentSet<T> : IContentSet where T : IContent
    {
        IEnumerable<T> Children { get; }
        int ChildCount { get; }

        void AddContent(IContent content);
        IContent GetContent(int index);
        IContent GetContent(string name);
        void Clear();
    }
}
