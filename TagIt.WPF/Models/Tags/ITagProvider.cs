using System;
using System.Collections.Generic;
using TagIt.WPF.Models.Videos;

namespace TagIt.WPF.Models.Tags
{
    /*public interface ITagProvider
    {
        IEnumerable<ITag> Tags { get; }
        IEnumerable<ITag> UncategorizedTags { get; }
        IEnumerable<string> Categories { get; }

        void AddFromLibrary(string filePath);
        void SaveToLibrary(string filePath);

        bool HasTag(string name);
        bool HasCategory(string name);

        ITag CreateTag(string name);
        void CreateCategory(string category);

        void RemoveTag(string name);
        void RemoveCategory(string category);

        void TagVideo(string tagName, IVideo video);
        void UntagVideo(string tagName, IVideo video);
        bool IsTagged(string tagName, IVideo video);

        void CategorizeTag(string tagName, string category);
        void UncategorizeTag(string tagName);
        bool IsCategorized(string tagName);

        ITag GetTag(string tagName);

        IEnumerable<ITag> GetTags(IVideo video);
        IEnumerable<ITag> GetTags(string category);

        event EventHandler<TagEventArgs> TagAdded;
        event EventHandler<EventArgs> CategoriesChanged;
        event EventHandler<EventArgs> TagsChanged;
    }*/
}
