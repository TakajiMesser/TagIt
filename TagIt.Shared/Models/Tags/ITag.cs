using System.Collections.Generic;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Tags
{
    public interface ITag
    {
        string Name { get; }
        string Category { get; set; }
        IEnumerable<IContent> Contents { get; }

        bool HasContent(IContent content);
        void AddContent(IContent content);
        void RemoveContent(string name);
        void Clear();
    }
}
