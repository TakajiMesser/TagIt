using System;
using System.Collections.Generic;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Tags
{
    public class TagManager// : ITagProvider
    {
        private static TagManager _instance;

        private TagManager() { }

        public static TagManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TagManager();
                }

                return _instance;
            }
        }

        private Dictionary<string, ITag> _tagByName = new Dictionary<string, ITag>();
        private Dictionary<string, HashSet<string>> _tagNamesByCategory = new Dictionary<string, HashSet<string>>();

        private HashSet<string> _uncategorizedTagNames = new HashSet<string>();

        public IEnumerable<ITag> Tags
        {
            get
            {
                foreach (var tag in _tagByName.Values)
                {
                    yield return tag;
                }
            }
        }

        public IEnumerable<ITag> UncategorizedTags
        {
            get
            {
                foreach (var tagName in _uncategorizedTagNames)
                {
                    yield return _tagByName[tagName];
                }
            }
        }

        public IEnumerable<string> Categories
        {
            get
            {
                foreach (var category in _tagNamesByCategory.Keys)
                {
                    yield return category;
                }
            }
        }

        public event EventHandler<TagEventArgs> TagAdded;
        public event EventHandler<EventArgs> CategoriesChanged;
        public event EventHandler<EventArgs> TagsChanged;

        public void AddFromLibrary(string filePath)
        {
            var tagLibrary = TagLibrary.Load(filePath);

            foreach (var tag in tagLibrary.Tags)
            {
                _tagByName.Add(tag.Name, tag);

                if (!string.IsNullOrEmpty(tag.Category))
                {
                    if (!_tagNamesByCategory.ContainsKey(tag.Category))
                    {
                        _tagNamesByCategory.Add(tag.Category, new HashSet<string>());
                    }

                    _tagNamesByCategory[tag.Category].Add(tag.Name);
                }
                else
                {
                    _uncategorizedTagNames.Add(tag.Name);
                }
            }
        }

        public void SaveToLibrary(string filePath)
        {
            var tagLibrary = new TagLibrary()
            {
                FilePath = filePath
            };

            foreach (var tag in Tags)
            {
                if (tag is Tag castTag)
                {
                    tagLibrary.Tags.Add(castTag);
                }
            }

            tagLibrary.Save();
        }

        public bool HasTag(string name) => _tagByName.ContainsKey(name);
        public bool HasCategory(string name) => _tagNamesByCategory.ContainsKey(name);

        public ITag CreateTag(string name)
        {
            var tag = new Tag(name);
            _tagByName.Add(name, tag);
            _uncategorizedTagNames.Add(name);

            TagAdded?.Invoke(this, new TagEventArgs(tag));
            TagsChanged?.Invoke(this, EventArgs.Empty);
            return tag;
        }

        public void CreateCategory(string category)
        {
            if (category == "Uncategorized") throw new ArgumentException("Reserved category name");

            _tagNamesByCategory.Add(category, new HashSet<string>());
            CategoriesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveTag(string name)
        {
            // TODO - Implement
            TagsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveCategory(string category)
        {
            // TODO - Don't even let the user attempt to remove the uncategorized category...
            if (category == "Uncategorized") throw new ArgumentException("Reserved category name");

            // TODO - Are you sure dialog if category contains tags/videos
            foreach (var tag in GetTags(category))
            {
                tag.Category = null;
                _uncategorizedTagNames.Add(tag.Name);
            }

            _tagNamesByCategory.Remove(category);
            CategoriesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void TagContent(string tagName, IContent content)
        {
            _tagByName[tagName].AddContent(content);
            TagsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UntagContent(string tagName, IContent content)
        {
            _tagByName[tagName].RemoveContent(content.Name);
            TagsChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool IsTagged(string tagName, IContent content) => _tagByName[tagName].HasContent(content);

        public void CategorizeTag(string tagName, string category)
        {
            if (!HasCategory(category))
            {
                CreateCategory(category);
            }

            _tagByName[tagName].Category = category;
            _tagNamesByCategory[category].Add(tagName);

            if (_uncategorizedTagNames.Contains(tagName))
            {
                _uncategorizedTagNames.Remove(tagName);
            }

            TagsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UncategorizeTag(string tagName)
        {
            var category = _tagByName[tagName].Category;
            _tagByName[tagName].Category = null;

            if (!string.IsNullOrEmpty(category))
            {
                _tagNamesByCategory[category].Remove(tagName);

                if (_tagNamesByCategory[category].Count == 0)
                {
                    _tagNamesByCategory.Remove(category);
                }
            }

            _uncategorizedTagNames.Add(tagName);
            TagsChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool IsCategorized(string tagName) => _uncategorizedTagNames.Contains(tagName);

        public ITag GetTag(string tagName) => _tagByName[tagName];

        public IEnumerable<ITag> GetTags(IContent content)
        {
            foreach (var tag in _tagByName.Values)
            {
                if (tag.HasContent(content))
                {
                    yield return tag;
                }
            }
        }

        public IEnumerable<ITag> GetTags(string category)
        {
            foreach (var tagName in _tagNamesByCategory[category])
            {
                yield return _tagByName[tagName];
            }
        }
    }
}
