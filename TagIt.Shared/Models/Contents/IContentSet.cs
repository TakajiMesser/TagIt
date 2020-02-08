using System.Collections.Generic;

namespace TagIt.Shared.Models.Contents
{
    public interface IContentSet<T> : IContentSet where T : IContent
    {
        IEnumerable<IContent> Children { get; }
        int Count { get; }

        void AddContent(IContent content);
        IContent GetContent(int index);
        IContent GetContent(string name);
        void Clear();
    }

    public interface IContentSet
    {
        
    }
}
