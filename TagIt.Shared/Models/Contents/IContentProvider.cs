using System.Collections.Generic;

namespace TagIt.Shared.Models.Contents
{
    public interface IContentProvider
    {
        IContent GetContent(int id);

        void AddContent(IContent content);
        void AddContents(IEnumerable<IContent> contents);
    }
}
